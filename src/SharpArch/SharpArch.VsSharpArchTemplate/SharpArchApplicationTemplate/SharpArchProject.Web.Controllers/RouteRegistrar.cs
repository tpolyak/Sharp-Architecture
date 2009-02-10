using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Web.Areas;

namespace $safeprojectname$
{
    public class RouteRegistrar
    {
        public static void RegisterRoutesTo(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            // The areas below must be registered from greater subareas to fewer;
            // i.e., the root area should be the last area registered

            // Example illustrative routes with a nested area - note that the order of registration is important
            //routes.CreateArea("Organization/Department", "$safeprojectname$.Organization.Department",
            //    routes.MapRoute(null, "Organization/Department/{controller}/{action}", new { action = "Index" })
            //);
            //routes.CreateArea("Organization", "$safeprojectname$.Organization",
            //    routes.MapRoute(null, "Organization/{controller}/{action}", new { action = "Index" }),
            //    routes.MapRoute(null, "Organization/{controller}/{action}/{id}")
            //);
            
            // Routing config for the root area
            routes.CreateArea("Root", "$safeprojectname$",
                routes.MapRoute(null, "{controller}/{action}", new { controller = "Home", action = "Index" })
            );
        }
    }
}
