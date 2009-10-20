using System.Web.Routing;

namespace SharpArch.Web.Areas
{
    public static class AreaRouteHelper
    {
        /// <summary>
        /// An extension method to facilitate the registration of area routes.  Described by Steve Sanderson
        /// at http://blog.codeville.net/2008/11/05/app-areas-in-aspnet-mvc-take-2/
        /// </summary>
        public static void CreateArea(this RouteCollection routes, string areaName, string controllersNamespace, params Route[] routeEntries) {
            foreach (var route in routeEntries) {
                if (route.Constraints == null) 
                    route.Constraints = new RouteValueDictionary();

                if (route.Defaults == null) 
                    route.Defaults = new RouteValueDictionary();
                
                if (route.DataTokens == null) 
                    route.DataTokens = new RouteValueDictionary();

                route.Constraints.Add("area", areaName);
                route.Defaults.Add("area", areaName);
                route.DataTokens.Add("namespaces", new string[] { controllersNamespace });

                if (!routes.Contains(route)) // To support "new Route()" in addition to "routes.MapRoute()"
                    routes.Add(route);
            }
        }
    }
}
