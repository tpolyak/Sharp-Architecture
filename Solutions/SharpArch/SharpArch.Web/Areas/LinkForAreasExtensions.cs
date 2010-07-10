namespace SharpArch.Web.Areas
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    using ExpressionHelper = Microsoft.Web.Mvc.Internal.ExpressionHelper;

    public static class LinkForAreasExtensions
    {
        /// <summary>
        ///     Length of the string ".dll"
        /// </summary>
        private const int LengthOfDotDll = 4;

        /// <summary>
        ///     Template to be used for the creation of area-supported links
        /// </summary>
        private const string LinkTemplate = "<a href=\"{0}{1}{2}{3}\">{4}</a>";

        /// <summary>
        ///     Creates an anchor tag based on the passed in controller type and method,
        ///     taking into account the area of the controller, as reflected in its namespace
        /// </summary>
        /// <typeparam name = "TController">The Controller Type</typeparam>
        /// <param name="helper">The HtmlHelper that the extension method applies to.</param>
        /// <param name = "action">The Method to route to</param>
        /// <param name = "linkText">The linked text to appear on the page</param>
        /// <returns>System.String</returns>
        public static string ActionLinkForAreas<TController>(
            this HtmlHelper helper, 
            Expression<Action<TController>> action, 
            string linkText)
            where TController : Controller
        {
            var controllerType = typeof(TController);
            var controllerUrlName = GetUrlNameEquivalentOf(controllerType);
            var areaUrl = ConvertNamespaceIntoAreaUrl(controllerType);

            var methodCall = GetMethodCall(action);
            var actionName = methodCall.Method.Name;
            var arguments = GetQueryStringArguments(helper, action, controllerUrlName, actionName);

            string path = helper.ViewContext.HttpContext.Request.ApplicationPath == "/"
                              ? helper.ViewContext.HttpContext.Request.ApplicationPath
                              : helper.ViewContext.HttpContext.Request.ApplicationPath + "/";
            
            return string.Format(
                LinkTemplate, 
                path, 
                !string.IsNullOrEmpty(areaUrl) ? (areaUrl + "/" + controllerUrlName) : controllerUrlName, 
                actionName == "Index" || actionName == string.Empty ? string.Empty : "/" + actionName, 
                arguments, 
                linkText);
        }

        public static string BuildUrlFromExpressionForAreas<TController>(
            this HtmlHelper helper, Expression<Action<TController>> action) where TController : Controller
        {
            var actionLink = ActionLinkForAreas(helper, action, "-");

            return GetRoutePortionFrom(actionLink);
        }

        /// <summary>
        ///     Translates the namespace of a controller into an area URL taking into account that the 
        ///     controller's assembly name should not be included as part of the URL
        /// </summary>
        private static string ConvertNamespaceIntoAreaUrl(Type controllerType)
        {
            return
                controllerType.Namespace.Replace(
                    controllerType.Module.Name.Substring(0, controllerType.Module.Name.Length - LengthOfDotDll), string.Empty).
                    TrimStart('.').Replace('.', '/');
        }

        private static MethodCallExpression GetMethodCall<T>(Expression<Action<T>> action)
        {
            var body = action.Body as MethodCallExpression;

            if (body == null)
            {
                throw new ArgumentException("Action must be a method call", "action");
            }
            
            return body;
        }

        private static string GetQueryStringArguments<TController>(
            HtmlHelper helper, 
            Expression<Action<TController>> action, 
            string controllerUrlName, 
            string actionName) where TController : Controller
        {
            var routingValues = ExpressionHelper.GetRouteValuesFromExpression(action);
            var routePortion =
                helper.RouteCollection.GetVirtualPath(helper.ViewContext.RequestContext, routingValues).VirtualPath;
            var controllerAndActionUrlPortion = controllerUrlName + "/" + actionName;

            // If there's a "?" in the querystring, then take everything from the "?" 
            // and onwards as the parameters to the URL
            if (routePortion.IndexOf('?') > -1)
            {
                return routePortion.Substring(routePortion.IndexOf('?'));
            }
                
            // If the controllerAndActionUrlPortion + "/" is found in the querystring, then it implies
            // that there are parameters, so take everything after the controllerAndActionUrlPortion, 
            // including the "/"
            if (routePortion.IndexOf(controllerAndActionUrlPortion + "/") > -1)
            {
                return
                    routePortion.Substring(
                        routePortion.IndexOf(controllerAndActionUrlPortion + "/") + controllerAndActionUrlPortion.Length);
            }
            
            return string.Empty;
        }

        private static string GetRoutePortionFrom(string anchorLink)
        {
            var startOfRoutePortion = anchorLink.IndexOf("href=") + 6;
            var endOfRoutePortion = anchorLink.IndexOf("\"", startOfRoutePortion);

            return anchorLink.Substring(startOfRoutePortion, endOfRoutePortion - startOfRoutePortion);
        }

        /// <summary>
        ///     The URL name eqivalent of a controller is its name sans the word "Controller" at the end
        /// </summary>
        private static string GetUrlNameEquivalentOf(Type controllerType)
        {
            return controllerType.Name.Substring(0, controllerType.Name.Length - "Controller".Length);
        }
    }
}