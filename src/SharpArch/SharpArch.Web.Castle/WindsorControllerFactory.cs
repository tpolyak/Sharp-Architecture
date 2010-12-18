// This class is a direct copy of the WindsorControllerFactory.cs in MVCContrib located at:
// http://mvccontrib.codeplex.com/SourceControl/changeset/view/b7039b7291cf#src%2fMvcContrib.Castle%2fWindsorControllerFactory.cs
//
// We chose to pull this in to S#arp Architecture as it was the only assembly from MVCContrib that had a dependency on Castle
// and we wanted to control that dependency

namespace SharpArch.Web.Castle
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using global::Castle.Windsor;

    /// <summary>
    /// Controller Factory class for instantiating controllers using the Windsor IoC container.
    /// </summary>
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private IWindsorContainer container;

        /// <summary>
        /// Creates a new instance of the <see cref="WindsorControllerFactory"/> class.
        /// </summary>
        /// <param name="container">The Windsor container instance to use when creating controllers.</param>
        public WindsorControllerFactory(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext context, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be found or it does not implement IController.", context.HttpContext.Request.Path));
            }

            return (IController)this.container.Resolve(controllerType);
        }

        public override void ReleaseController(IController controller)
        {
            var disposable = controller as IDisposable;

            if (disposable != null)
            {
                disposable.Dispose();
            }

            this.container.Release(controller);
        }
    }
}