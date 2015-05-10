namespace SharpArch.Web.Mvc.Castle
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Domain.Reflection;
    using global::Castle.Core;
    using global::Castle.MicroKernel;
    using global::Castle.MicroKernel.ComponentActivator;
    using global::Castle.MicroKernel.Registration;
    using global::Castle.Windsor;

    public static class WindsorExtensions
    {
        /// <summary>
        ///     Searches for the first interface found associated with the
        ///     <see cref="ServiceDescriptor" /> which is not generic and which
        ///     is found in the specified namespace.
        /// </summary>
        public static BasedOnDescriptor FirstNonGenericCoreInterface(
            this ServiceDescriptor descriptor, string interfaceNamespace)
        {
            return descriptor.Select(
                delegate(Type type, Type[] baseType)
                {
                    var interfaces =
                        type.GetInterfaces().Where(
                            t => t.IsGenericType == false && t.Namespace.StartsWith(interfaceNamespace));

                    if (interfaces.Any())
                    {
                        return new[] {interfaces.ElementAt(0)};
                    }

                    return null;
                });
        }

        public static IWindsorContainer RegisterController<T>(this IWindsorContainer container) where T : IController
        {
            container.RegisterControllers(typeof (T));
            return container;
        }

        public static IWindsorContainer RegisterControllers(this IWindsorContainer container,
            params Type[] controllerTypes)
        {
            foreach (var type in controllerTypes)
            {
                if (ControllerExtensions.IsController(type))
                {
                    container.Register(
                        Component.For(type).Named(type.FullName.ToLower()).LifeStyle.Is(LifestyleType.Transient));
                }
            }

            return container;
        }

        public static IWindsorContainer RegisterControllers(this IWindsorContainer container,
            params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                container.RegisterControllers(assembly.GetExportedTypes());
            }
            return container;
        }

        /// <summary>
        /// Injects dependencies into properties.
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="target">The target object to inject properties.</param>
        /// <param name="cache">Injectable property descriptor cache.</param>
        /// <exception cref="ComponentActivatorException"></exception>
        public static void InjectProperties(this IKernel kernel, object target, ITypePropertyDescriptorCache cache)
        {
            if (target == null) throw new ArgumentNullException("target");

            var type = target.GetType();

            var info = cache != null
                ? cache.Find(type) ?? cache.GetOrAdd(type, () => GetInjectableProperties(type, kernel))
                : GetInjectableProperties(type, kernel);


            if (!info.HasProperties()) return;

            for (int i = 0; i < info.Properties.Length; i++)
            {
                var injectableProperty = info.Properties[i];
                var val = kernel.Resolve(injectableProperty.PropertyType);
                try
                {
                    injectableProperty.SetValue(target, val, null);
                }

                catch (Exception ex)
                {
                    var message =
                        string.Format(
                            "Error setting property {0} on type {1}, See inner exception for more information.",
                            injectableProperty.Name, type.FullName);

                    throw new ComponentActivatorException(message, ex, null);
                }
            }
        }

        private static TypePropertyDescriptor GetInjectableProperties(Type type, IKernel kernel)
        {
            var injectableProperties = (from prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where prop.CanWrite 
                    && !prop.PropertyType.IsValueType
                    && kernel.HasComponent(prop.PropertyType)
                select prop
                ).ToArray();
            return new TypePropertyDescriptor(type,
                injectableProperties.Any() ? injectableProperties : null);
        }
    }
}
