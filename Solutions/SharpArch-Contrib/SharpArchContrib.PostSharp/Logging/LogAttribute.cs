using System;
using log4net;
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
    public class LogAttribute : OnMethodBoundaryAspect {
        private IMethodLogger methodLogger;

        public LogAttribute() {
            Settings = new LogAttributeSettings(LoggingLevel.Debug, LoggingLevel.Debug, LoggingLevel.Error);
        }

        public LoggingLevel EntryLevel {
            get { return Settings.EntryLevel; }
            set { Settings.EntryLevel = value; }
        }

        public LoggingLevel SuccessLevel {
            get { return Settings.SuccessLevel; }
            set { Settings.SuccessLevel = value; }
        }

        public LoggingLevel ExceptionLevel {
            get { return Settings.ExceptionLevel; }
            set { Settings.ExceptionLevel = value; }
        }

        public LogAttributeSettings Settings { get; set; }

        private IMethodLogger MethodLogger {
            get {
                if (methodLogger == null) {
                    methodLogger = ServiceLocator.Current.GetInstance<IMethodLogger>();
                }
                return methodLogger;
            }
        }

        public override void OnEntry(MethodExecutionEventArgs eventArgs) {
            MethodLogger.LogEntry(eventArgs.Method, eventArgs.GetReadOnlyArgumentArray(), EntryLevel);
        }

        public override void OnSuccess(MethodExecutionEventArgs eventArgs) {
            methodLogger.LogSuccess(eventArgs.Method, eventArgs.ReturnValue, SuccessLevel);
        }

        public override void OnException(MethodExecutionEventArgs eventArgs) {
            methodLogger.LogException(eventArgs.Method, eventArgs.Exception, ExceptionLevel);
        }

        private bool ShouldLog(ILog logger, LoggingLevel loggingLevel, MethodExecutionEventArgs args) {
            if (args != null && args.Method != null && args.Method.Name != null) {
                return logger.IsEnabledFor(loggingLevel);
            }

            return false;
        }
    }
}