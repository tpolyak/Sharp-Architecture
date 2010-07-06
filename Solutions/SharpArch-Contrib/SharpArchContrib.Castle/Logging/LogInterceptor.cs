using System;
using System.Reflection;
using Castle.Core.Interceptor;
using SharpArchContrib.Core;
using SharpArchContrib.Core.Logging;

namespace SharpArchContrib.Castle.Logging {
    public class LogInterceptor : IInterceptor {
        private readonly IMethodLogger methodLogger;

        public LogInterceptor(IMethodLogger methodLogger) {
            ParameterCheck.ParameterRequired(methodLogger, "methodLogger");

            this.methodLogger = methodLogger;
        }

        #region IInterceptor Members

        public void Intercept(IInvocation invocation) {
            MethodInfo methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null) {
                methodInfo = invocation.Method;
            }

            //we take the most permissive log settings from the attributes we find
            //If there is at least one attribute, the call gets wrapped with a transaction
            var assemblyLogAttributes =
                (LogAttribute[]) methodInfo.ReflectedType.Assembly.GetCustomAttributes(typeof(LogAttribute), false);
            var classLogAttributes =
                (LogAttribute[]) methodInfo.ReflectedType.GetCustomAttributes(typeof(LogAttribute), false);
            var methodLogAttributes =
                (LogAttribute[]) methodInfo.GetCustomAttributes(typeof(LogAttribute), false);

            if (assemblyLogAttributes.Length == 0 && classLogAttributes.Length == 0 && methodLogAttributes.Length == 0) {
                invocation.Proceed();
            }
            else {
                LogAttributeSettings logAttributeSettings = GetLoggingLevels(assemblyLogAttributes, classLogAttributes,
                                                                             methodLogAttributes);
                methodLogger.LogEntry(methodInfo, invocation.Arguments, logAttributeSettings.EntryLevel);
                try {
                    invocation.Proceed();
                }
                catch (Exception err) {
                    methodLogger.LogException(methodInfo, err, logAttributeSettings.ExceptionLevel);
                    throw;
                }
                methodLogger.LogSuccess(methodInfo, invocation.ReturnValue, logAttributeSettings.SuccessLevel);
            }
        }

        #endregion

        private LogAttributeSettings GetLoggingLevels(LogAttribute[] assemblyLogAttributes,
                                                      LogAttribute[] classLogAttributes,
                                                      LogAttribute[] methodLogAttributes) {
            var logAttributeSettings = new LogAttributeSettings();
            logAttributeSettings = GetLoggingLevels(assemblyLogAttributes, logAttributeSettings);
            logAttributeSettings = GetLoggingLevels(classLogAttributes, logAttributeSettings);
            logAttributeSettings = GetLoggingLevels(methodLogAttributes, logAttributeSettings);

            return logAttributeSettings;
        }

        private LogAttributeSettings GetLoggingLevels(LogAttribute[] logAttributes,
                                                      LogAttributeSettings logAttributeSettings) {
            foreach (LogAttribute logAttribute in logAttributes) {
                if (logAttribute.Settings.EntryLevel > logAttributeSettings.EntryLevel) {
                    logAttributeSettings.EntryLevel = logAttribute.Settings.EntryLevel;
                }
                if (logAttribute.Settings.SuccessLevel > logAttributeSettings.SuccessLevel) {
                    logAttributeSettings.SuccessLevel = logAttribute.Settings.SuccessLevel;
                }
                if (logAttribute.Settings.ExceptionLevel > logAttributeSettings.ExceptionLevel) {
                    logAttributeSettings.ExceptionLevel = logAttribute.Settings.ExceptionLevel;
                }
            }

            return logAttributeSettings;
        }
    }
}