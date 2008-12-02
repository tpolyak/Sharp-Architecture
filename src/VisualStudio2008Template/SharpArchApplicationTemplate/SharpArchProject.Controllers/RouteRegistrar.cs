using System.Web.Mvc;
using System.Web.Routing;

namespace $safeprojectname$
{
    public class RouteRegistrar
    {
        public static void RegisterRoutesTo(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" }
            );
        }
    }
}
