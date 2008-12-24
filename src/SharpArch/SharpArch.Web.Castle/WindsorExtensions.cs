using Castle.MicroKernel.Registration;
using System;
using System.Linq;
using Castle.Windsor;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.MetaData;
using Castle.Core;

namespace SharpArch.Web.Castle
{
    public static class WindsorExtensions
    {
        /// <summary>
        /// Searches for the first interface found associated with the 
        /// <see cref="ServiceDescriptor" /> which is not generic and which 
        /// is found in the specified namespace.
        /// </summary>
        public static BasedOnDescriptor FirstNonGenericCoreInterface(this ServiceDescriptor descriptor, string interfaceNamespace) {
            return descriptor.Select(delegate(Type type, Type baseType) {
                var interfaces = type.GetInterfaces()
                    .Where(t => t.IsGenericType == false && t.Namespace.StartsWith(interfaceNamespace));
                
                if (interfaces.Count() > 0) {
                    return new[] { interfaces.ElementAt(0) };
                }
                
                return null;
            });
        }

        /// <summary>
        /// This is a modified version of the RegisterControllers extension found within MvcContrib.
        /// This modified version registers the controller by its full name to account for areas.
        /// </summary>
        public static IWindsorContainer RegisterController<T>(this IWindsorContainer container) where T : IController {
            container.RegisterControllersByArea(typeof(T));
            return container;
        }

        /// <summary>
        /// This is a modified version of the RegisterControllers extension found within MvcContrib.
        /// This modified version registers the controller by its full name to account for areas.
        /// </summary>
        public static IWindsorContainer RegisterControllersByArea(this IWindsorContainer container, params Type[] controllerTypes) {
            foreach (var type in controllerTypes) {
                if (ControllerDescriptor.IsController(type)) {
                    container.AddComponentLifeStyle(type.FullName.ToLower(), type, LifestyleType.Transient);
                }
            }

            return container;
        }

        /// <summary>
        /// This is a modified version of the RegisterControllers extension found within MvcContrib.
        /// This modified version registers the controller by its full name to account for areas.
        /// </summary>
        public static IWindsorContainer RegisterControllersByArea(this IWindsorContainer container, params Assembly[] assemblies) {
            foreach (var assembly in assemblies) {
                container.RegisterControllersByArea(assembly.GetExportedTypes());
            }

            return container;
        }
    }
}
