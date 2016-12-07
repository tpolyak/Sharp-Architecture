namespace SharpArch.Web.Http.Castle
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dependencies;
    using System.Web.Http.Filters;
    using Domain.Reflection;
    using global::Castle.Windsor;
    using JetBrains.Annotations;

    /// <summary>
    ///     Castle Windsor configuration for WebAPI.
    /// </summary>
    [PublicAPI]
    public static class WindsorHttpConfigurationExtensions
    {
        /// <summary>
        ///     Installs Filter provider with injectable property dependencies support.
        /// </summary>
        /// <param name="services">Services container, <see cref="ServicesContainer" /></param>
        /// <param name="container">Windsor container, <see cref="IWindsorContainer" /> </param>
        /// <param name="propertyDescriptorCache">Injectable property cache</param>
        /// <returns>Windsor container</returns>
        public static void InstallHttpFilterProvider([NotNull] this ServicesContainer services,
            [NotNull] IWindsorContainer container, ITypePropertyDescriptorCache propertyDescriptorCache)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (container == null) throw new ArgumentNullException(nameof(container));

            var providers = services.GetFilterProviders().Where(i => i is ActionDescriptorFilterProvider).ToArray();
            foreach (var filterProvider in providers)
            {
                services.Remove(typeof (IFilterProvider), filterProvider);
            }

            services.Add(typeof (IFilterProvider), new WindsorHttpFilterProvider(container, propertyDescriptorCache));
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
        ///                 Add property injection support for ActionFilters, <see cref="WindsorHttpFilterProvider" />
        ///             </description>
        ///         </item>
        ///     </list>
        /// </remarks>
        /// <returns>
        ///     <see cref="HttpConfiguration" />
        /// </returns>
        public static HttpConfiguration UseWindsor(this HttpConfiguration configuration, IWindsorContainer container,
            ITypePropertyDescriptorCache injectablePropertyCache)
        {
            configuration.DependencyResolver = new WindsorDependencyResolver(container);
            configuration.Services.InstallHttpFilterProvider(container, injectablePropertyCache);
            return configuration;
        }
    }
}