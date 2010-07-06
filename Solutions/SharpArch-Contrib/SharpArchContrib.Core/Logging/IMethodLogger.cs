using System;
using System.Reflection;

namespace SharpArchContrib.Core.Logging {
    public interface IMethodLogger {
        void LogEntry(MethodBase methodBase, object[] argumentValues, LoggingLevel entryLevel);
        void LogSuccess(MethodBase methodBase, object returnValue, LoggingLevel successLevel);
        void LogException(MethodBase methodBase, Exception err, LoggingLevel exceptionLevel);
    }
}