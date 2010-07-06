using System;
using System.Reflection;
using System.Text;
using log4net;

namespace SharpArchContrib.Core.Logging {
    public class MethodLogger : IMethodLogger {
        private readonly IExceptionLogger exceptionLogger;

        public MethodLogger(IExceptionLogger exceptionLogger) {
            ParameterCheck.ParameterRequired(exceptionLogger, "exceptionLogger");

            this.exceptionLogger = exceptionLogger;
        }

        #region IMethodLogger Members

        public void LogEntry(MethodBase methodBase, object[] argumentValues, LoggingLevel entryLevel) {
            ILog logger = LogManager.GetLogger(methodBase.DeclaringType);
            if (ShouldLog(logger, entryLevel, methodBase)) {
                var logMessage = new StringBuilder();
                logMessage.Append(string.Format("{0}(", methodBase.Name));

                ParameterInfo[] parameterInfos = methodBase.GetParameters();
                if (argumentValues != null && parameterInfos != null) {
                    for (int i = 0; i < argumentValues.Length; i++) {
                        if (i > 0) {
                            logMessage.Append(" ");
                        }
                        logMessage.Append(string.Format("{0}:[{1}]", parameterInfos[i].Name, argumentValues[i]));
                    }
                }
                logMessage.Append(")");
                logger.Log(LoggingLevel.Debug, logMessage.ToString());
            }
        }

        public void LogSuccess(MethodBase methodBase, object returnValue, LoggingLevel successLevel) {
            ILog logger = LogManager.GetLogger(methodBase.DeclaringType);
            if (ShouldLog(logger, successLevel, methodBase)) {
                logger.Log(LoggingLevel.Debug,
                           string.Format("{0} Returns:[{1}]", methodBase.Name,
                                         returnValue != null ? returnValue.ToString() : ""));
            }
        }

        public void LogException(MethodBase methodBase, Exception err, LoggingLevel exceptionLevel) {
            ILog logger = LogManager.GetLogger(methodBase.DeclaringType);
            if (ShouldLog(logger, exceptionLevel, methodBase)) {
                exceptionLogger.LogException(err, false, methodBase.DeclaringType);
            }
        }

        #endregion

        private bool ShouldLog(ILog logger, LoggingLevel loggingLevel, MethodBase methodBase) {
            if (methodBase != null && methodBase.Name != null) {
                return logger.IsEnabledFor(loggingLevel);
            }

            return false;
        }
    }
}