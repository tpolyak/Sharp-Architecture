namespace Suteki.TardisBank.Web.Mvc
{
    using System;
    using System.Reflection;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using log4net.Config;
    using SharpArch.Domain.Reflection;
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
        private TypePropertyDescriptorCache propertyDescriptorCache;
        private IWindsorContainer container;

        protected void Application_Error(object sender, EventArgs e)
        {
            // Useful for debugging
            var ex = Server.GetLastError();
            var reflectionTypeLoadException = ex as ReflectionTypeLoadException;
        }

        protected void Application_Start()
        {
            XmlConfigurator.Configure();

            AutoMapper.Mapper.Initialize(c =>
            {
                c.AddProfile<ModelMappingProfile>();
            });


            // Container
            propertyDescriptorCache = new TypePropertyDescriptorCache();
            container = InitializeConianer();

            // MVC
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            ModelBinders.Binders.DefaultBinder = new SharpModelBinder();
            FilterProviders.Providers.InstallFilterProvider(container, propertyDescriptorCache);
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            AreaRegistration.RegisterAllAreas();

            // WepAPI
            GlobalConfiguration.Configure(cfg => WebApiConfig.Register(cfg, container, propertyDescriptorCache));

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        /// <summary>
        ///     Instantiate the container and add all Controllers that derive from
        ///     WindsorController to the container.  Also associate the Controller
        ///     with the WindsorContainer ControllerFactory.
        /// </summary>
        protected IWindsorContainer InitializeConianer()
        {
            IWindsorContainer c = new WindsorContainer();
            c.Install(FromAssembly.This());
            return c;

        }


        protected void Application_AuthenticateRequest(Object sender,
        EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    FormsIdentity id = HttpContext.Current.User.Identity as FormsIdentity;
                    if (id != null)
                    {
                        var ticket = id.Ticket;

                        // Get the stored user-data, in this case, our roles
                        string userData = ticket.UserData;
                        string[] roles = userData.Split(',');
                        HttpContext.Current.User = new GenericPrincipal(id, roles);
                    }
                }
            }
        }
    }
}
