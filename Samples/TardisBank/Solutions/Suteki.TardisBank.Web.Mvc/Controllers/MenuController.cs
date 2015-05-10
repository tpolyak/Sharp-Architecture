namespace Suteki.TardisBank.Web.Mvc.Controllers
{
    using System.Web.Mvc;

    using Suteki.TardisBank.Domain;
    using Suteki.TardisBank.Tasks;

    public class MenuController : Controller
    {
        readonly IUserService userService;

        public MenuController(IUserService userService)
        {
            this.userService = userService;
        }

        [ChildActionOnly]
        public ViewResult Index()
        {
            var user = this.userService.CurrentUser;

            if (user == null) return this.View("GuestMenu");
            if (user is Parent) return this.View("ParentMenu", user as Parent);
            if (user is Child) return this.View("ChildMenu", user as Child);

            throw new TardisBankException("Unknown User type");
        }
    }
}