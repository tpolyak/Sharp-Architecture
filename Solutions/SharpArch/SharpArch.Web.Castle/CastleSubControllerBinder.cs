using MvcContrib.Binders;
using Castle.Windsor;
using System;
using SharpArch.Core;

namespace SharpArch.Web.Castle
{
    public class CastleSubControllerBinder : SubControllerBinder
    {
        public CastleSubControllerBinder(IWindsorContainer container) {
            Check.Require(container != null, "container may not be null");

            this.container = container;
        }

        public override object CreateSubController(Type destinationType) {
            object instance = container.Resolve(destinationType);
            Check.Ensure(instance != null, destinationType + " not registered with CastleWindsor");

            return instance;
        }

        private IWindsorContainer container;
    }
}
