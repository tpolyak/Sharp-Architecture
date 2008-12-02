using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;
using NHibernate.Cfg;
using Castle.Windsor;
using Northwind.Controllers;
using MvcContrib.Castle;
using Castle.MicroKernel.Registration;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core.PersistenceSupport.NHibernate;
using Northwind.Web.CastleWindsor;
using SharpArch.Data.NHibernate;
using SharpArch.Web.NHibernate;

namespace Northwind.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication, IContainerAccessor
    {
        protected void Application_Start() {
            log4net.Config.XmlConfigurator.Configure();

            InitializeWindsor();
            RouteRegistrar.RegisterRoutesTo(RouteTable.Routes);
        }

        /// <summary>
        /// Instantiate the container and add all Controllers that derive from 
        /// WindsorController to the container.  Also associate the Controller 
        /// with the WindsorContainer ControllerFactory.
        /// </summary>
        protected virtual void InitializeWindsor() {
            if (container == null) {
                container = new WindsorContainer();

                ControllerBuilder.Current.SetControllerFactory(
                    new MvcContrib.Castle.WindsorControllerFactory(Container));
                container.RegisterControllers(typeof(HomeController).Assembly);

                ComponentRegistrar.AddComponentsTo(container);
            }
        }

        public override void Init() {
            base.Init();

            NHibernateSession.Init(new WebSessionStorage(this), 
                new string[] { Server.MapPath("~/bin/Northwind.Data.dll") });
        }

        protected void Application_Error(object sender, EventArgs e) {
            // Useful for debugging
            Exception ex = Server.GetLastError();
            ReflectionTypeLoadException reflectionTypeLoadException = ex as ReflectionTypeLoadException;
        }

        public static IWindsorContainer Container {
            get { return container; }
        }

        IWindsorContainer IContainerAccessor.Container {
            get { return Container; }
        }

        private static WindsorContainer container;
    }
}