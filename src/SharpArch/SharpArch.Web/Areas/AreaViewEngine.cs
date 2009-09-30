using System.Web.Mvc;

namespace SharpArch.Web.Areas
{
    /// <summary>
    /// Replacement view engine to support the organization of views into areas.  Taken from the
    /// sample provided by Phil Haack at http://haacked.com/archive/2008/11/04/areas-in-aspnetmvc.aspx.
    /// It's been modified to allow area views to sit within subfolders under /Views
    /// </summary>
    public class AreaViewEngine : WebFormViewEngine
    {
        public AreaViewEngine()
            : base() {
            ViewLocationFormats = new[] { 
                "~/{0}.aspx",
                "~/{0}.ascx",
                "~/Views/{1}/{0}.aspx",
                "~/Views/{1}/{0}.ascx",
                "~/Views/Shared/{0}.aspx",
                "~/Views/Shared/{0}.ascx",
            };

            MasterLocationFormats = new[] {
                "~/{0}.master",
                "~/Shared/{0}.master",
                "~/Views/{1}/{0}.master",
                "~/Views/Shared/{0}.master",
            };

            PartialViewLocationFormats = ViewLocationFormats;
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache) {
            ViewEngineResult areaResult = null;

            if (controllerContext.RouteData.Values.ContainsKey("area")) {
                string areaPartialName = FormatViewName(controllerContext, partialViewName);
                areaResult = base.FindPartialView(controllerContext, areaPartialName, useCache);

                if (areaResult != null && areaResult.View != null) {
                    return areaResult;
                }
                
                string sharedAreaPartialName = FormatSharedViewName(controllerContext, partialViewName);
                areaResult = base.FindPartialView(controllerContext, sharedAreaPartialName, useCache);
                
                if (areaResult != null && areaResult.View != null) {
                    return areaResult;
                }
            }

            return base.FindPartialView(controllerContext, partialViewName, useCache);
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache) {

            ViewEngineResult areaResult = null;

            if (controllerContext.RouteData.Values.ContainsKey("area")) {
                string areaViewName = FormatViewName(controllerContext, viewName);
                areaResult = base.FindView(controllerContext, areaViewName, masterName, useCache);

                if (areaResult != null && areaResult.View != null) {
                    return areaResult;
                }
                
                string sharedAreaViewName = FormatSharedViewName(controllerContext, viewName);
                areaResult = base.FindView(controllerContext, sharedAreaViewName, masterName, useCache);
                
                if (areaResult != null && areaResult.View != null) {
                    return areaResult;
                }
            }

            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        private static string FormatViewName(ControllerContext controllerContext, string viewName) {
            string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            string area = controllerContext.RouteData.Values["area"].ToString();
            return "Views/" + area + "/" + controllerName + "/" + viewName;
        }

        private static string FormatSharedViewName(ControllerContext controllerContext, string viewName) {
            string area = controllerContext.RouteData.Values["area"].ToString();
            return "Views/" + area + "/Shared/" + viewName;
        }
    }
}
