using Castle.MicroKernel.Registration;
using System;
using System.Linq;

namespace SharpArch.Web.Castle
{
    using System.Reflection;
    using System.Web.Mvc;

    using global::Castle.Core;
    using global::Castle.Windsor;

    using MvcContrib;

    public static class WindsorExtensions
    {
        /// <summary>
        /// Searches for the first interface found associated with the 
        /// <see cref="ServiceDescriptor" /> which is not generic and which 
        /// is found in the specified namespace.
        /// </summary>
        public static BasedOnDescriptor FirstNonGenericCoreInterface(this ServiceDescriptor descriptor, string interfaceNamespace)
        {
            return descriptor.Select(
                delegate(Type type, Type[] baseType)
                {
                    var interfaces =
                        type.GetInterfaces().Where(
                            t => t.IsGenericType == false && t.Namespace.StartsWith(interfaceNamespace));

                    if (interfaces.Count() > 0)
                    {
                        return new[] { interfaces.ElementAt(0) };
                    }

                    return null;
                });
        }

        public static IWindsorContainer RegisterController<T>(this IWindsorContainer container) where T : IController
        {
            container.RegisterControllers(typeof(T));
            return container;
        }

        public static IWindsorContainer RegisterControllers(this IWindsorContainer container, params Type[] controllerTypes)
        {
            foreach (var type in controllerTypes)
            {
                if (ControllerExtensions.IsController(type))
                {
                    container.AddComponentLifeStyle(type.FullName.ToLower(), type, LifestyleType.Transient);
                }
            }

            return container;
        }

        public static IWindsorContainer RegisterControllers(this IWindsorContainer container, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                container.RegisterControllers(assembly.GetExportedTypes());
            }
            return container;
        }
    }
}
