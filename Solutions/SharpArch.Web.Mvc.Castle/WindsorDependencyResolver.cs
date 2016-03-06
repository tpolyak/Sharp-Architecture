//--------------------------------------------
// Implementation provided by StackOverflow
// http://stackoverflow.com/questions/4140860/castle-windsor-dependency-resolver-for-mvc-3-rc
//--------------------------------------------

namespace SharpArch.Web.Mvc.Castle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using global::Castle.Windsor;
    using JetBrains.Annotations;

    /// <summary>
    /// Resolves dependency from <see cref="IWindsorContainer"/>.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.IDependencyResolver" />
    [PublicAPI]
    public class WindsorDependencyResolver : IDependencyResolver
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentNullException"><see paramref="container"/> is null.</exception>
        public WindsorDependencyResolver([NotNull] IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            this.container = container;
        }

        /// <summary>
        /// Resolves singly registered services that support arbitrary object creation.
        /// </summary>
        /// <param name="serviceType">The type of the requested service or object.</param>
        /// <returns>
        /// The requested service or object.
        /// </returns>
        public object GetService(Type serviceType)
        {
            return container.Kernel.HasComponent(serviceType) ? container.Resolve(serviceType) : null;
        }

        /// <summary>
        /// Resolves multiply registered services.
        /// </summary>
        /// <param name="serviceType">The type of the requested services.</param>
        /// <returns>
        /// The requested services.
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.Kernel.HasComponent(serviceType) ? container.ResolveAll(serviceType).Cast<object>() : Enumerable.Empty<object>();
        }
    }
}