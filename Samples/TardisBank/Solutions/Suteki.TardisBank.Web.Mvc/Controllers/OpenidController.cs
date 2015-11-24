namespace Suteki.TardisBank.Web.Mvc.Controllers
{
    using System.Web.Mvc;

    using DotNetOpenAuth.OpenId.RelyingParty;

    using Suteki.TardisBank.Domain;
    using Suteki.TardisBank.Tasks;
    using Suteki.TardisBank.Web.Mvc.Utilities;

    public class OpenidController : Controller
    {
        readonly IFormsAuthenticationService formsAuthenticationService;
        readonly IOpenIdService openIdService;
        readonly IUserService userService;

        public OpenidController(
            IFormsAuthenticationService formsAuthenticationService, 
            IOpenIdService openIdService, 
            IUserService userService)
        {
            this.formsAuthenticationService = formsAuthenticationService;
            this.userService = userService;
            this.openIdService = openIdService;
        }


        public ActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Login", new {ReturnUrl = "Index"});
            }

            return this.View("Index");
        }

        public ActionResult Logout()
        {
            this.formsAuthenticationService.SignOut();
            return this.Redirect("~/Home");
        }

        public ActionResult Login()
        {
            // Stage 1: display login form to user
            return this.View("Login");
        }

        [ValidateInput(false)]
        public ActionResult Authenticate(string returnUrl)
        {
            var response = this.openIdService.GetResponse();

            // Stage 2: Make the request to the openId provider
            if (response == null)
            {
                try
                {
                    return this.openIdService.CreateRequest(this.Request.Form["openid_identifier"]);
                }
                catch (OpenIdException openIdException)
                {
                    this.ViewData["Message"] = openIdException.Message;
                    return this.View("Login");
                }
            }
            
            // Stage 3: OpenID Provider sending assertion response
            switch (response.Status)
            {
                case AuthenticationStatus.Authenticated:
                    this.formsAuthenticationService.SetAuthCookie(response.ClaimedIdentifier, false);

                    this.CreateNewParentIfTheyDontAlreadyExist(response);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return this.Redirect(returnUrl);
                    }
                    return this.RedirectToAction("Index", "Home");
                case AuthenticationStatus.Canceled:
                    this.ViewData["Message"] = "Canceled at provider";
                    return this.View("Login");
                case AuthenticationStatus.Failed:
                    this.ViewData["Message"] = response.Exception.Message;
                    return this.View("Login");
            }

            throw new TardisBankException("Unknown AuthenticationStatus Response");
        }

        void CreateNewParentIfTheyDontAlreadyExist(IAuthenticationResponse response)
        {
            var userName = response.ClaimedIdentifier;
            if (this.userService.GetUserByUserName(userName) != null) return;

            var parent = new Parent(response.FriendlyIdentifierForDisplay, response.ClaimedIdentifier, "todo");
            this.userService.SaveUser(parent);
        }
    }
}