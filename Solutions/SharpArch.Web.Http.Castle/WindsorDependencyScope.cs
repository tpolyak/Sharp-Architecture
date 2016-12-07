// ReSharper disable WordCanBeSurroundedWithMetaTags
namespace SharpArch.Web.Http.Castle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Web.Http.Dependencies;
    using global::Castle.MicroKernel.Lifestyle;
    using global::Castle.Windsor;
    using JetBrains.Annotations;

    /// <summary>
    /// Dependency scope for Windsor container.
    /// </summary>
    /// <seealso cref="System.Web.Http.Dependencies.IDependencyScope" />
    class WindsorDependencyScope : IDependencyScope
    {
        readonly IWindsorContainer container;
        IDisposable scope;

        /// <summary>
        /// </summary>
        /// <param name="container">Windsor container.</param>
        public WindsorDependencyScope([NotNull] IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            this.container = container;
            this.scope = container.BeginScope();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.scope?.Dispose();
            this.scope = null;
        }

        /// <summary>
        ///     Retrieves a service from the container.
        /// </summary>
        /// <param name="serviceType">The service to be retrieved.</param>
        /// <returns>
        ///     The retrieved service.
        /// </returns>
        public object GetService(Type serviceType)
        {
            return TryResolveService(this.container, serviceType);
        }

        /// <summary>
        ///     Retrieves a collection of services from the container.
        /// </summary>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        /// <returns>
        ///     The retrieved collection of services.
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return TryResolveServices(this.container, serviceType);
        }


        /// <summary>
        ///     Tries to resolve service.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>Service instance or <c>null</c> if it cannot be resolved by the container.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static object TryResolveService(IWindsorContainer container, Type serviceType)
        {
            if (!container.Kernel.HasComponent(serviceType))
            {
                return null;
            }

            return container.Resolve(serviceType);
        }

        /// <summary>
        ///     Tries to resolve services.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IEnumerable<object> TryResolveServices(IWindsorContainer container, Type serviceType)
        {
            if (!container.Kernel.HasComponent(serviceType))
            {
                return Enumerable.Empty<object>();
            }

            // ReSharper disable once SuspiciousTypeConversion.Global
            return (IEnumerable<object>)container.ResolveAll(serviceType);
        }
    }
}