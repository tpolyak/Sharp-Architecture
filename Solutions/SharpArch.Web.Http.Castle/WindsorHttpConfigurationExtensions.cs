namespace SharpArch.Web.Http.Castle
{
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dependencies;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Filters;
    using Domain;
    using Domain.Reflection;
    using global::Castle.Windsor;

    /// <summary>
    ///     Castle Windsor configuration for WebAPI.
    /// </summary>
    public static class WindsorHttpConfigurationExtensions
    {
        /// <summary>
        ///     Installs Filter provider with injectable property dependencies support.
        /// </summary>
        /// <param name="services">Services container, <see cref="ServicesContainer" /></param>
        /// <param name="container">Windsor container, <see cref="IWindsorContainer" /> </param>
        /// <param name="propertyDescriptorCache">Injectable property cache</param>
        /// <returns>Windsor container</returns>
        public static void InstallFilterProvider(this ServicesContainer services,
            IWindsorContainer container, ITypePropertyDescriptorCache propertyDescriptorCache)
        {
            Check.Require(services != null);
            Check.Require(container != null);
            Check.Require(propertyDescriptorCache != null);

            var providers = services.GetFilterProviders().Where(i => i is ActionDescriptorFilterProvider).ToArray();
            foreach (var filterProvider in providers)
            {
                services.Remove(typeof (IFilterProvider), filterProvider);
            }

            services.Add(typeof (IFilterProvider), new WindsorFilterProvider(container, propertyDescriptorCache));
        }

        /// <summary>
        ///     Resolve controllers from Windsor container.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static void InstallControllerActivator(this ServicesContainer services,
            IWindsorContainer container)
        {
            services.Replace(typeof (IHttpControllerActivator), new WindsorHttpControllerActivator(container));
        }

        /// <summary>
        ///     Configures Web API runtime to use Castle Windsor Container
        /// </summary>
        /// <param name="configuration">Configuration</param>
        /// <param name="container">Windsor container to use</param>
        /// <param name="injectablePropertyCache">Injectable property cache</param>
        /// <remarks>
        ///     Performs following actions
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Install DependencyResolver <see cref="IDependencyResolver" /></description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 Add injectable properties support to ActionFilters, <see cref="WindsorFilterProvider" />
        ///             </description>
        ///         </item>
        ///     </list>
        ///     <item>
        ///         <description>
        ///             Install Windsor controller activator, <see cref="WindsorHttpControllerActivator" />
        ///         </description>
        ///     </item>
        /// </remarks>
        /// <returns>
        ///     <see cref="HttpConfiguration" />
        /// </returns>
        public static HttpConfiguration UseWindsor(this HttpConfiguration configuration, IWindsorContainer container,
            ITypePropertyDescriptorCache injectablePropertyCache)
        {
            configuration.DependencyResolver = new WindsorDependencyResolver(container);
            configuration.Services.InstallFilterProvider(container, injectablePropertyCache);
            configuration.Services.InstallControllerActivator(container);

            return configuration;
        }
    }
}