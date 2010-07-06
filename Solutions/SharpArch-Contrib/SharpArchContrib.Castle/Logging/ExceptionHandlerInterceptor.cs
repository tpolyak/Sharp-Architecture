using System;
using System.Reflection;
using Castle.Core.Interceptor;
using SharpArchContrib.Core;
using SharpArchContrib.Core.Logging;

namespace SharpArchContrib.Castle.Logging {
    public class ExceptionHandlerInterceptor : IInterceptor {
        private readonly IExceptionLogger exceptionLogger;

        public ExceptionHandlerInterceptor(IExceptionLogger exceptionLogger) {
            ParameterCheck.ParameterRequired(exceptionLogger, "exceptionLogger");

            this.exceptionLogger = exceptionLogger;
        }

        #region IInterceptor Members

        public void Intercept(IInvocation invocation) {
            MethodInfo methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null) {
                methodInfo = invocation.Method;
            }

            //we take the settings from the first attribute we find searching method first
            //If there is at least one attribute, the call gets wrapped with an exception handler
            var assemblyAttributes =
                (ExceptionHandlerAttribute[])
                methodInfo.ReflectedType.Assembly.GetCustomAttributes(typeof(ExceptionHandlerAttribute), false);
            var classAttributes =
                (ExceptionHandlerAttribute[])
                methodInfo.ReflectedType.GetCustomAttributes(typeof(ExceptionHandlerAttribute), false);
            var methodAttributes =
                (ExceptionHandlerAttribute[]) methodInfo.GetCustomAttributes(typeof(ExceptionHandlerAttribute), false);

            if (assemblyAttributes.Length == 0 && classAttributes.Length == 0 && methodAttributes.Length == 0) {
                invocation.Proceed();
            }
            else {
                ExceptionHandlerAttributeSettings exceptionHandlerAttributeSettings =
                    GetExceptionHandlerSettings(assemblyAttributes, classAttributes, methodAttributes);
                try {
                    invocation.Proceed();
                }
                catch (Exception err) {
                    exceptionLogger.LogException(err, exceptionHandlerAttributeSettings.IsSilent,
                                                 methodInfo.ReflectedType);
                    if (exceptionHandlerAttributeSettings.IsSilent) {
                        if (exceptionHandlerAttributeSettings.ExceptionType == null ||
                            exceptionHandlerAttributeSettings.ExceptionType == err.GetType())
                        {
                            invocation.ReturnValue = exceptionHandlerAttributeSettings.ReturnValue;
                        }
                        else
                        {
                            throw;
                        }
                    }
                    else {
                        throw;
                    }
                }
            }
        }

        #endregion

        private ExceptionHandlerAttributeSettings GetExceptionHandlerSettings(
            ExceptionHandlerAttribute[] assemblyAttributes, ExceptionHandlerAttribute[] classAttributes,
            ExceptionHandlerAttribute[] methodAttributes) {
            if (methodAttributes.Length > 0) {
                return methodAttributes[0].Settings;
            }
            if (classAttributes.Length > 0) {
                return classAttributes[0].Settings;
            }
            return assemblyAttributes[0].Settings;
        }
    }
}