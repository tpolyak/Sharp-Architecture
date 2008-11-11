using System.Web.Mvc;

namespace Northwind.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index() {
            return View();
        }
    }
}
