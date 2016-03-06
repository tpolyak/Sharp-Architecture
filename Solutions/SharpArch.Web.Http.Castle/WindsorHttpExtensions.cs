namespace SharpArch.Web.Http.Castle
{
    using System;
    using System.Linq;
    using System.Reflection;
    using global::Castle.Core;
    using global::Castle.MicroKernel.Registration;
    using global::Castle.Windsor;
    using JetBrains.Annotations;

    /// <summary>
    /// Contains Castle Windsor related HTTP controller extension methods.
    /// </summary>
    [PublicAPI]
    public static class WindsorHttpExtensions
    {
        /// <summary>
        /// Registers the specified WebAPI controllers.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="controllerTypes">The controller types.</param>
        /// <returns>
        /// A container.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        [NotNull]
        public static IWindsorContainer RegisterHttpControllers([NotNull] this IWindsorContainer container, params Type[] controllerTypes)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            foreach (Type type in controllerTypes.Where(type => type.IsHttpController()))
            {
                container.Register(
                    Component.For(type).Named(type.FullName).LifeStyle.Is(LifestyleType.Scoped));
            }

            return container;
        }

        /// <summary>
        /// Registers the WebAPI controllers from specified assemblies.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>
        /// A container.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        [NotNull]
        public static IWindsorContainer RegisterHttpControllers([NotNull] this IWindsorContainer container, params Assembly[] assemblies)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            foreach (Assembly assembly in assemblies)
            {
                RegisterHttpControllers(container, assembly.GetExportedTypes());
            }

            return container;
        }

    }
}
