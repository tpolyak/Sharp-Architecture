using Castle.Windsor;
using SharpArchContrib.Core.Logging;

namespace SharpArchContrib.Core.CastleWindsor {
    public static class ComponentRegistrar {
        public static void AddComponentsTo(IWindsorContainer container) {
            ParameterCheck.ParameterRequired(container, "container");

            if (!container.Kernel.HasComponent("ExceptionLogger")) {
                container.AddComponent("ExceptionLogger", typeof(IExceptionLogger), typeof(ExceptionLogger));
                container.AddComponent("MethodLogger", typeof(IMethodLogger), typeof(MethodLogger));
            }
        }
    }
}