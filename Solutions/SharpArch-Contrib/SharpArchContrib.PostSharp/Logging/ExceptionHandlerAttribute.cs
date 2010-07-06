using System;
using Microsoft.Practices.ServiceLocation;
using PostSharp.Extensibility;
using PostSharp.Laos;
using SharpArchContrib.Core.Logging;

namespace SharpArchContrib.PostSharp.Logging {
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false,
        Inherited = false)]
    [MulticastAttributeUsage(
        MulticastTargets.Method | MulticastTargets.InstanceConstructor | MulticastTargets.StaticConstructor,
        AllowMultiple = true)]
    public sealed class ExceptionHandlerAttribute : OnExceptionAspect {
        private IExceptionLogger exceptionLogger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ExceptionHandlerAttribute() {
            Settings = new ExceptionHandlerAttributeSettings();
        }

        public bool IsSilent {
            get { return Settings.IsSilent; }
            set { Settings.IsSilent = value; }
        }

        public object ReturnValue {
            get { return Settings.ReturnValue; }
            set { Settings.ReturnValue = value; }
        }

        public ExceptionHandlerAttributeSettings Settings { get; set; }

        private IExceptionLogger ExceptionLogger {
            get {
                if (exceptionLogger == null) {
                    exceptionLogger = ServiceLocator.Current.GetInstance<IExceptionLogger>();
                }
                return exceptionLogger;
            }
        }

        public override void OnException(MethodExecutionEventArgs eventArgs) {
            ExceptionLogger.LogException(eventArgs.Exception, IsSilent, eventArgs.Method.DeclaringType);
            if (IsSilent) {
                eventArgs.FlowBehavior = FlowBehavior.Return;
                eventArgs.ReturnValue = ReturnValue;
            }
        }
    }
}