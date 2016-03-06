namespace SharpArch.Web.Mvc.Castle
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Domain.Reflection;
    using global::Castle.Core;
    using global::Castle.MicroKernel.Registration;
    using global::Castle.Windsor;
    using JetBrains.Annotations;

    /// <summary>
    /// ASP.NET MVC extensions for Castle Windsor.
    /// </summary>
    [PublicAPI]
    public static class WindsorMvcExtensions
    {
        /// <summary>
        ///     Searches for the first interface found associated with the
        ///     <see cref="ServiceDescriptor" /> which is not generic and which
        ///     is found in the specified namespace.
        /// </summary>
        [CanBeNull]
        public static BasedOnDescriptor FirstNonGenericCoreInterface([NotNull] this ServiceDescriptor descriptor, string interfaceNamespace)
        {
            if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));
            return descriptor.Select((type, baseType) =>
                {
                    var intf = type.GetInterfaces()
                            .FirstOrDefault(t => t.IsGenericType == false && t.Namespace.StartsWith(interfaceNamespace));
                    return intf != null ? new[] {intf} : null;
                });
        }

        /// <summary>
        ///     Register ASP.NET MVC controller.
        /// </summary>
        /// <typeparam name="T">Controller</typeparam>
        /// <param name="container">Windsor container.</param>
        /// <returns>Windsor container</returns>
        [NotNull]
        public static IWindsorContainer RegisterMvcController<T>(this IWindsorContainer container) where T : IController
        {
            container.RegisterMvcControllers(typeof (T));
            return container;
        }

        /// <summary>
        ///     Register ASP.NET MVC controllers
        /// </summary>
        /// <param name="container">Windsor container</param>
        /// <param name="controllerTypes">Controller types</param>
        /// <returns>Windsor container</returns>
        [NotNull]
        public static IWindsorContainer RegisterMvcControllers([NotNull] this IWindsorContainer container,
            params Type[] controllerTypes)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            foreach (var type in controllerTypes)
            {
                if (ControllerExtensions.IsController(type))
                {
                    container.Register(
                        Component.For(type).Named(type.FullName).LifeStyle.Is(LifestyleType.Transient));
                }
            }

            return container;
        }

        /// <summary>
        ///     Add injectable dependencies support to FilterAttributetes.
        /// </summary>
        /// <remarks>
        ///     Replaces <see cref="FilterAttributeFilterProvider" /> with
        /// </remarks>
        /// <param name="container">The container.</param>
        /// <param name="propertyDescriptorCache"></param>
        /// <param name="filterProviders">The filter providers.</param>
        /// <returns>Windsor container</returns>
        [NotNull]
        public static IWindsorContainer InstallMvcFilterProvider(
            [NotNull] this FilterProviderCollection filterProviders, [NotNull] IWindsorContainer container,
            [CanBeNull] TypePropertyDescriptorCache propertyDescriptorCache)
        {
            if (filterProviders == null) throw new ArgumentNullException(nameof(filterProviders));
            if (container == null) throw new ArgumentNullException(nameof(container));
            var attributeFilterProviders = filterProviders.OfType<FilterAttributeFilterProvider>().ToArray();
            foreach (var attributeFilterProvider in attributeFilterProviders)
            {
                filterProviders.Remove(attributeFilterProvider);
            }
            filterProviders.Add(new WindsorFilterAttributeProvider(container, propertyDescriptorCache));
            return container;
        }

        /// <summary>
        ///     Register ASP.NET MVC controllers from given assemblies.
        /// </summary>
        /// <param name="container">Windsor container</param>
        /// <param name="assemblies">Assemblies to scan</param>
        /// <returns>Windsor container</returns>
        [NotNull]
        public static IWindsorContainer RegisterMvcControllers(this IWindsorContainer container,
            params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                container.RegisterMvcControllers(assembly.GetExportedTypes());
            }
            return container;
        }
    }
}