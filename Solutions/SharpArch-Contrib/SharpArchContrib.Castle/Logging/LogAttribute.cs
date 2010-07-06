using System;
using SharpArchContrib.Core.Logging;

namespace SharpArchContrib.Castle.Logging {
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false,
        Inherited = false)]
    public class LogAttribute : Attribute {
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
    }
}