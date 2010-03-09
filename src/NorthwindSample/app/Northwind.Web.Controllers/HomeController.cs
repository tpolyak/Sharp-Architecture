using System.Web.Mvc;

namespace Northwind.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Alec"] = "Alec";
            return View();
        }
    }
}
