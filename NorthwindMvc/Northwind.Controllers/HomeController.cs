using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Northwind.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() {
            return View("Index");
        }
    }
}
