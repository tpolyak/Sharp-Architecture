using System;

namespace SharpArchContrib.Core.Logging {
    [Serializable]
    public class LogAttributeSettings {
        public LogAttributeSettings() : this(LoggingLevel.Off, LoggingLevel.Off, LoggingLevel.Off) {}

        public LogAttributeSettings(LoggingLevel entryLevel, LoggingLevel successLevel, LoggingLevel exceptionLevel) {
            EntryLevel = entryLevel;
            SuccessLevel = successLevel;
            ExceptionLevel = exceptionLevel;
        }

        public LoggingLevel EntryLevel { get; set; }
        public LoggingLevel SuccessLevel { get; set; }
        public LoggingLevel ExceptionLevel { get; set; }
    }
}