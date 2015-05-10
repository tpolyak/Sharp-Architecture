namespace Suteki.TardisBank.Web.Mvc
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using CastleWindsor;
    using Infrastructure.NHibernateMaps;
    using log4net.Config;
    using Microsoft.Practices.ServiceLocation;
    using SharpArch.Domain.Events;
    using SharpArch.Domain.Reflection;
    using SharpArch.NHibernate;
    using SharpArch.NHibernate.Web.Mvc;
    using SharpArch.Web.Mvc.Castle;
    using SharpArch.Web.Mvc.ModelBinder;

    /// <summary>
    ///     Represents the MVC Application
    /// </summary>
    /// <remarks>
    ///     For instructions on enabling IIS6 or IIS7 classic mode,
    ///     visit http://go.microsoft.com/?LinkId=9394801
    /// </remarks>
    public class MvcApplication : HttpApplication
    {
        private WebSessionStorage webSessionStorage;

        /// <summary>
        ///     Due to issues on IIS7, the NHibernate initialization must occur in Init().
        ///     But Init() may be invoked more than once; accordingly, we introduce a thread-safe
        ///     mechanism to ensure it's only initialized once.
        ///     See http://msdn.microsoft.com/en-us/magazine/cc188793.aspx for explanation details.
        /// </summary>
        public override void Init()
        {
            base.Init();
            webSessionStorage = new WebSessionStorage(this);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            NHibernateInitializer.Instance().InitializeNHibernateOnce(InitialiseNHibernateSessions);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Useful for debugging
            var ex = Server.GetLastError();
            var reflectionTypeLoadException = ex as ReflectionTypeLoadException;
        }

        protected void Application_Start()
        {
            XmlConfigurator.Configure();

            ViewEngines.Engines.Clear();

            ViewEngines.Engines.Add(new RazorViewEngine());

            ModelBinders.Binders.DefaultBinder = new SharpModelBinder();

            InitializeServiceLocator();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        ///     Instantiate the container and add all Controllers that derive from
        ///     WindsorController to the container.  Also associate the Controller
        ///     with the WindsorContainer ControllerFactory.
        /// </summary>
        protected virtual void InitializeServiceLocator()
        {
            IWindsorContainer container = new WindsorContainer();
            container.Install(FromAssembly.This());

            InstallFilterProvider(container);

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            var serviceLocator = new MvcServiceLocator();
            DomainEvents.ServiceLocator = serviceLocator;

            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        private static void InstallFilterProvider(IWindsorContainer container)
        {
            var attributeFilterProviders = FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().ToArray();
            foreach (var attributeFilterProvider in attributeFilterProviders)
            {
                FilterProviders.Providers.Remove(attributeFilterProvider);
            }
            FilterProviders.Providers.Add(new WindsorFilterAttributeProvider(container, new TypePropertyDescriptorCache()));
        }

        private void InitialiseNHibernateSessions()
        {
            NHibernateSession.ConfigurationCache = new NHibernateConfigurationFileCache();

            NHibernateSession.Init(
                webSessionStorage,
                new[] {Server.MapPath("~/bin/Suteki.TardisBank.Infrastructure.dll")},
                new AutoPersistenceModelGenerator().Generate(),
                Server.MapPath("~/NHibernate.config"));
        }
    }
}
