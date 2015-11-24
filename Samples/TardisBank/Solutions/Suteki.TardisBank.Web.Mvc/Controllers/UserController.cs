namespace Suteki.TardisBank.Web.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;

    using Suteki.TardisBank.Domain;
    using Suteki.TardisBank.Tasks;
    using Suteki.TardisBank.Web.Mvc.Controllers.ViewModels;
    using Suteki.TardisBank.Web.Mvc.Utilities;

    public class UserController : Controller
    {
        readonly IUserService userService;
        readonly IFormsAuthenticationService formsAuthenticationService;
        private readonly TardisConfiguration configuration;

        public UserController(IUserService userService, IFormsAuthenticationService formsAuthenticationService, TardisConfiguration configuration)
        {
            this.userService = userService;
            this.formsAuthenticationService = formsAuthenticationService;
            this.configuration = configuration;
        }

        [ChildActionOnly]
        public ActionResult Index()
        {
            var user = this.userService.CurrentUser;
            var userName = user == null ? "Hello Stranger!" : user.UserName;
            return this.View("Index", new UserViewModel { UserName = userName, IsLoggedIn = user != null });
        }

        [HttpGet]
        public ActionResult Register()
        {
            return this.View("Register", GetRegistrationViewModel());
        }

        static RegistrationViewModel GetRegistrationViewModel()
        {
            return new RegistrationViewModel
            {
                Email = "",
                Name = "",
                Password = ""
            };
        }

        [HttpPost, SharpArch.Web.Mvc.Transaction]
        public ActionResult Register(RegistrationViewModel registrationViewModel)
        {
            return this.RegisterInternal(registrationViewModel, "Sorry, that email address has already been registered.",
                createUser: (pwd) => new Parent(registrationViewModel.Name, registrationViewModel.Email, pwd).Initialise(),
                confirmAction: () => this.RedirectToAction("Confirm"),
                invalidModelStateAction: () => this.View("Register", registrationViewModel)
                );
        }

        ActionResult RegisterInternal(
            RegistrationViewModel registrationViewModel,
            string usernameTakenMessage,
            Func<string, User> createUser,
            Func<ActionResult> confirmAction, 
            Func<ActionResult> invalidModelStateAction,
            Action<User> afterUserCreated = null)
        {
            if (registrationViewModel == null)
            {
                throw new ArgumentNullException("registrationViewModel");
            }
            if (createUser == null)
            {
                throw new ArgumentNullException("createUser");
            }
            if (confirmAction == null)
            {
                throw new ArgumentNullException("confirmAction");
            }
            if (invalidModelStateAction == null)
            {
                throw new ArgumentNullException("invalidModelStateAction");
            }

            if (this.ModelState.IsValid)
            {
                var conflictedUser = this.userService.GetUserByUserName(registrationViewModel.Email);
                if (conflictedUser != null)
                {
                    this.ModelState.AddModelError("Email", usernameTakenMessage);
                    return invalidModelStateAction();
                }

                var hashedPassword = this.formsAuthenticationService.HashAndSalt(
                    registrationViewModel.Email,
                    registrationViewModel.Password);

                var user = createUser(hashedPassword);

                if (string.IsNullOrWhiteSpace(this.configuration.EmailSmtpServer))
                {
                    // if no smtp server configured, just activate user as no email is sent out.
                    user.Activate();
                }

                this.userService.SaveUser(user);

                if (afterUserCreated != null)
                {
                    afterUserCreated(user);
                }
                return confirmAction();
            }

            return invalidModelStateAction();
        }

        [HttpGet]
        public ActionResult Confirm()
        {
            return this.View("Confirm");
        }

        [HttpGet, SharpArch.Web.Mvc.Transaction]
        public ActionResult Activate(string id)
        {
            // id is the activation key
            var user = this.userService.GetUserByActivationKey(id);
            if (user == null)
            {
                return this.View("ActivationFailed");
            }
            user.Activate();
            return this.View("ActivateConfirm");
        }

        [HttpGet]
        public ActionResult Login()
        {
            var loginViewModel = new LoginViewModel
            {
                Name = "",
                Password = ""
            };
            return this.View("Login", loginViewModel);
        }

        [HttpPost, SharpArch.Web.Mvc.Transaction]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (loginViewModel == null)
            {
                throw new ArgumentNullException("loginViewModel");
            }

            if (this.ModelState.IsValid)
            {
                var user = this.userService.GetUserByUserName(loginViewModel.Name);
                if (user != null)
                {
                    if (!user.IsActive)
                    {
                        this.ModelState.AddModelError(
                            "Name", "Please activate your account first by clicking on the link in your " + 
                            "activation email.");
                        return this.View("Login", loginViewModel);
                    }

                    var hashedPassword = this.formsAuthenticationService.HashAndSalt(
                        loginViewModel.Name,
                        loginViewModel.Password);

                    if(hashedPassword == user.Password)
                    {
                        this.formsAuthenticationService.SetAuthCookie(user.UserName, false);
                        if (user is Child)
                        {
                            return this.RedirectToAction("ChildView", "Account");
                        }
                        else
                        {
                            return this.RedirectToAction("Index", "Child");
                        }
                    }
                    this.ModelState.AddModelError("Password", "Invalid Password");
                }
                else
                {
                    this.ModelState.AddModelError("Name", "Invalid Name");
                }
            }

            return this.View("Login", loginViewModel);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            this.formsAuthenticationService.SignOut();
            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet, SharpArch.Web.Mvc.Transaction]
        public ActionResult AddChild()
        {
            var parent = this.userService.CurrentUser as Parent;
            if (parent == null)
            {
                //throw new TardisBankException("You must be a parent in order to register a Child");
                return StatusCode.NotFound;
            }

            return this.View("AddChild", GetRegistrationViewModel());
        }

        [HttpPost, SharpArch.Web.Mvc.Transaction]
        public ActionResult AddChild(RegistrationViewModel registrationViewModel)
        {
            var parent = this.userService.CurrentUser as Parent;
            if(parent == null)
            {
                //throw new TardisBankException("You must be a parent in order to register a Child");
                return StatusCode.NotFound;
            }

            return this.RegisterInternal(registrationViewModel, "Sorry, that user name has already been taken",
                createUser: (pwd) => parent.CreateChild(registrationViewModel.Name, registrationViewModel.Email, pwd),
                confirmAction: () => this.RedirectToAction("Index", "Child"),
                invalidModelStateAction: () => this.View("AddChild", registrationViewModel)
                );
        }

        [HttpGet, SharpArch.Web.Mvc.Transaction]
        public ActionResult Messages()
        {
            var user = this.userService.CurrentUser;
            if (user == null)
            {
                return StatusCode.NotFound;
            }
            return this.View("Messages", user);
        }

        [HttpGet, SharpArch.Web.Mvc.Transaction]
        public ActionResult ReadMessage(int id)
        {
            var user = this.userService.CurrentUser;
            if (user == null)
            {
                return StatusCode.NotFound;
            }
            user.ReadMessage(id);
            return this.RedirectToAction("Messages");
        }
    }
}