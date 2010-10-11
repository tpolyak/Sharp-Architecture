namespace SharpArch.Web.Castle
{
    using System;

    using global::Castle.Windsor;

    using MvcContrib.Binders;

    using SharpArch.Core;

    public class CastleSubControllerBinder : SubControllerBinder
    {
        private readonly IWindsorContainer container;

        public CastleSubControllerBinder(IWindsorContainer container)
        {
            Check.Require(container != null, "container may not be null");

            this.container = container;
        }

        public override object CreateSubController(Type destinationType)
        {
            var instance = this.container.Resolve(destinationType);
            Check.Ensure(instance != null, destinationType + " not registered with CastleWindsor");

            return instance;
        }
    }
}