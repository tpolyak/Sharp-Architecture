using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ProjectBase.Data.NHibernate;
using ProjectBase.Web.NHibernate;
using ProjectBase.Web;
using System.Reflection;

namespace Northwind.Web
{
    public class GlobalApplication : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e) {
            NHibernateSession.Init(new WebSessionStorage(), null);
            ControllerBuilder.Current.SetControllerFactory(typeof(ControllerFactory));

            RegisterRoutes(RouteTable.Routes);
        }

        public void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}.mvc/{action}/{id}",                       // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }

        protected void Application_Error(object sender, EventArgs e) {
            // Useful for debugging
            Exception ex = Server.GetLastError();
            ReflectionTypeLoadException reflectionTypeLoadException = ex as ReflectionTypeLoadException;
        }
    }
}