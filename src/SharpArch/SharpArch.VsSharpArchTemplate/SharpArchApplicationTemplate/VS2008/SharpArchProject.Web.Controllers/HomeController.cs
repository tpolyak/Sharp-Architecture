using System.Web.Mvc;

namespace $safeprojectname$
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index() {
            return View();
        }
    }
}
