namespace Suteki.TardisBank.Web.Mvc
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using Castle.MicroKernel;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Installer;

    using Infrastructure.NHibernateMaps;

    using log4net.Config;

    using NHibernate;

    using SharpArch.Domain.Reflection;
    using SharpArch.NHibernate;
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

        ISessionFactory CreateSessionFactory(IKernel kernel)
        {
            ISessionFactory sessionFactory = new NHibernateSessionFactoryBuilder()
                .AddMappingAssemblies(new[] {HostingEnvironment.MapPath("~/bin/Suteki.TardisBank.Infrastructure.dll")})
                .UseAutoPersitenceModel(new AutoPersistenceModelGenerator().Generate())
                .UseConfigFile(HostingEnvironment.MapPath("~/NHibernate.config"))
                .UseConfigurationCache(new NHibernateConfigurationFileCache())
                .BuildSessionFactory();

            return sessionFactory;
        }

        /// <summary>
        ///     Instantiate the container and add all Controllers that derive from
        ///     WindsorController to the container.  Also associate the Controller
        ///     with the WindsorContainer ControllerFactory.
        /// </summary>
        protected virtual void InitializeServiceLocator()
        {
            IWindsorContainer container = new WindsorContainer();

            container.Register(
                Component.For<ISessionFactory>()
                    .UsingFactoryMethod(CreateSessionFactory)
                    .LifestyleSingleton()
                    .Named(NHibernateSessionFactoryBuilder.DefaultConfigurationName + ".factory")
                );

            container.Register(
                Component.For<ISession>()
                    .UsingFactoryMethod(k => k.Resolve<ISessionFactory>().OpenSession())
                    .LifestylePerWebRequest()
                    .Named(NHibernateSessionFactoryBuilder.DefaultConfigurationName + ".session")
                    .OnDestroy(s => Debug.WriteLine("Destroy session {0:x}", s.GetHashCode()))
                    .OnCreate(s => Debug.WriteLine("Created session {0:x}", s.GetHashCode()))
                );

            container.Register(
                Component.For<IStatelessSession>()
                    .UsingFactoryMethod(k => k.Resolve<ISessionFactory>().OpenStatelessSession())
                    .LifestylePerWebRequest()
                    .Named(NHibernateSessionFactoryBuilder.DefaultConfigurationName + ".stateless-session")
                );

            container.Install(FromAssembly.This());

            InstallFilterProvider(container);

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            //var serviceLocator = new MvcServiceLocator();
            //DomainEvents.ServiceLocator = serviceLocator;

            //ServiceLocator.SetLocatorProvider(() => serviceLocator);
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

    }
}
