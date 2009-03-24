using System.Web.Mvc;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Microsoft.Web.Mvc.Internal;

namespace SharpArch.Web.Areas
{
    public static class LinkForAreasExtensions
    {
        /// <summary>
        /// Creates an anchor tag based on the passed in controller type and method,
        /// taking into account the area of the controller, as reflected in its namespace
        /// </summary>
        /// <typeparam name="TController">The Controller Type</typeparam>
        /// <param name="action">The Method to route to</param>
        /// <param name="linkText">The linked text to appear on the page</param>
        /// <returns>System.String</returns>
        public static string ActionLinkForAreas<TController>(this HtmlHelper helper,
            Expression<Action<TController>> action, string linkText) where TController : Controller {

            Type controllerType = typeof(TController);
            string controllerUrlName = GetUrlNameEquivalentOf(controllerType);
            string areaUrl = ConvertNamespaceIntoAreaUrl(controllerType);

            MethodCallExpression methodCall = GetMethodCall(action);
            string actionName = methodCall.Method.Name;
            string arguments = GetQueryStringArguments(helper, action, linkText);

            return string.Format(linkTemplate,
                helper.ViewContext.HttpContext.Request.ApplicationPath == "/" 
                    ? helper.ViewContext.HttpContext.Request.ApplicationPath
                    : helper.ViewContext.HttpContext.Request.ApplicationPath + "/",
                !string.IsNullOrEmpty(areaUrl)
                    ? (areaUrl + "/" + controllerUrlName)
                    : controllerUrlName,
                actionName == "Index" || actionName == ""
                    ? ""
                    : "/" + actionName,
                arguments,
                linkText);
        }

        public static string BuildUrlFromExpressionForAreas<TController>(this HtmlHelper helper,
            Expression<Action<TController>> action) where TController : Controller {

            string actionLink = ActionLinkForAreas<TController>(helper, action, "-");

            return GetRoutePortionFrom(actionLink);
        }

        private static string GetQueryStringArguments<TController>(HtmlHelper helper, Expression<Action<TController>> action, string linkText) where TController : Controller {
            RouteValueDictionary routingValues = ExpressionHelper.GetRouteValuesFromExpression(action);
            string routeLinkFromMvc = helper.RouteLink(linkText, routingValues);

            string routePortion = GetRoutePortionFrom(routeLinkFromMvc);

            if (routePortion.IndexOf('?') > -1) {
                return routePortion.Substring(routePortion.IndexOf('?'));
            }
            else {
                return "";
            }
        }

        private static string GetRoutePortionFrom(string anchorLink) {
            int startOfRoutePortion = anchorLink.IndexOf("href=") + 6;
            int endOfRoutePortion = anchorLink.IndexOf("\"", startOfRoutePortion);

            return
                anchorLink.Substring(startOfRoutePortion, endOfRoutePortion - startOfRoutePortion);
        }

        /// <summary>
        /// The URL name eqivalent of a controller is its name sans the word "Controller" at the end
        /// </summary>
        private static string GetUrlNameEquivalentOf(Type controllerType) {
            return controllerType.Name.Substring(0, controllerType.Name.Length - "Controller".Length);
        }

        /// <summary>
        /// Translates the namespace of a controller into an area URL taking into account that the 
        /// controller's assembly name should not be included as part of the URL
        /// </summary>
        private static string ConvertNamespaceIntoAreaUrl(Type controllerType) {
            return controllerType.Namespace
                    .Replace(controllerType.Module.Name.Substring(0, controllerType.Module.Name.Length - LENGTH_OF_DOT_DLL), "")
                    .TrimStart('.').Replace('.', '/');
        }

        private static MethodCallExpression GetMethodCall<T>(Expression<Action<T>> action) {
            MethodCallExpression body = action.Body as MethodCallExpression;

            if (body == null)
                throw new ArgumentException("Action must be a method call", "action");
            else
                return body;
        }

        /// <summary>
        /// Template to be used for the creation of area-supported links
        /// </summary>
        private const string linkTemplate = "<a href=\"{0}{1}{2}{3}\">{4}</a>";

        /// <summary>
        /// Length of the string ".dll"
        /// </summary>
        private const int LENGTH_OF_DOT_DLL = 4;
    }
}
