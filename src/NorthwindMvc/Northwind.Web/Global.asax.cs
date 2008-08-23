using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Ninject;
using Northwind.Web.NinjectModules;
using ProjectBase.Data.NHibernate;
using ProjectBase.Web.NHibernate;
using System.Reflection;
using ProjectBase.Web.Ninject;

namespace Northwind.Web
{
    public class GlobalApplication : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e) {
            InitializeNinject();
            RegisterRoutes(RouteTable.Routes);
        }

        private void InitializeNinject() {
            NinjectKernel.Initialize(new ControllersAutoBindModule("Northwind.Controllers"), new DataModule());
            ControllerBuilder.Current.SetControllerFactory(typeof(MvcContrib.Ninject.NinjectControllerFactory));
        }

        public override void Init() {
            base.Init();

            NHibernateSession.Init(new WebSessionStorage(this), null);
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