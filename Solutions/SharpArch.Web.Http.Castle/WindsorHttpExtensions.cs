namespace SharpArch.Web.Http.Castle
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Filters;
    using Domain.Reflection;
    using global::Castle.Core;
    using global::Castle.MicroKernel.Registration;
    using global::Castle.Windsor;
    using SharpArch.Domain;

    /// <summary>
    /// Contains Castle Windsor related HTTP controller extension methods.
    /// </summary>
    public static class WindsorHttpExtensions
    {
        /// <summary>
        /// Registers the specified HTTP controllers.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="controllerTypes">The controller types.</param>
        /// <returns>A container.</returns>
        public static IWindsorContainer RegisterControllers(this IWindsorContainer container, params Type[] controllerTypes)
        {
            Check.Require(container != null);
            Check.Require(controllerTypes != null);

            foreach (Type type in controllerTypes.Where(ControllerExtensions.IsHttpController))
            {
                container.Register(
                    Component.For(type).Named(type.FullName).LifeStyle.Is(LifestyleType.Transient));
            }

            return container;
        }

        /// <summary>
        /// Registers the HTTP controllers from specified assemblies.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>A container.</returns>
        public static IWindsorContainer RegisterControllers(this IWindsorContainer container, params Assembly[] assemblies)
        {
            Check.Require(container != null);
            Check.Require(assemblies != null);

            foreach (Assembly assembly in assemblies)
            {
                RegisterControllers(container, assembly.GetExportedTypes());
            }

            return container;
        }

    }
}
