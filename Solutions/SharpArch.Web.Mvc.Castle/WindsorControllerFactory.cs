namespace SharpArch.Web.Mvc.Castle
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using global::Castle.Windsor;
    using JetBrains.Annotations;

    /// <summary>
    /// Controller Factory class for instantiating controllers using the Windsor IoC container.
    /// </summary>
    [PublicAPI]
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private IWindsorContainer container;

        /// <summary>
        /// Creates a new instance of the <see cref="WindsorControllerFactory"/> class.
        /// </summary>
        /// <param name="container">The Windsor container instance to use when creating controllers.</param>
        public WindsorControllerFactory([NotNull] IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            this.container = container;
        }

        /// <summary>
        /// Disposes controller.
        /// </summary>
        /// <param name="controller">The controller to release.</param>
        public override void ReleaseController([CanBeNull] IController controller)
        {
            var disposable = controller as IDisposable;
            disposable?.Dispose();

            this.container.Release(controller);
        }

        /// <summary>
        /// Resolves controller.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns></returns>
        /// <exception cref="System.Web.HttpException">Controller type can not be resolved.</exception>
        protected override IController GetControllerInstance(RequestContext context, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404,
                    $"The controller for path '{context.HttpContext.Request.Path}' could not be found or it does not implement IController.");
            }

            return (IController)this.container.Resolve(controllerType);
        }
    }
}