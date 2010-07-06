using System;
using System.Reflection;
using Castle.Core.Interceptor;
using SharpArch.Data.NHibernate;
using SharpArchContrib.Core;
using SharpArchContrib.Core.Logging;
using SharpArchContrib.Data.NHibernate;

namespace SharpArchContrib.Castle.NHibernate {
    public class TransactionInterceptor : IInterceptor {
        protected readonly IExceptionLogger exceptionLogger;
        protected readonly ITransactionManager transactionManager;

        public TransactionInterceptor(ITransactionManager transactionManager, IExceptionLogger exceptionLogger) {
            ParameterCheck.ParameterRequired(transactionManager, "transactionManager");
            ParameterCheck.ParameterRequired(exceptionLogger, "exceptionLogger");

            this.transactionManager = transactionManager;
            this.exceptionLogger = exceptionLogger;
        }

        #region IInterceptor Members

        public void Intercept(IInvocation invocation) {
            MethodInfo methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null) {
                methodInfo = invocation.Method;
            }

            //we take the settings from the first attribute we find searching method first
            //If there is at least one attribute, the call gets wrapped with a transaction
            Type attributeType = GetAttributeType();
            var classAttributes =
                (ITransactionAttributeSettings[])
                methodInfo.ReflectedType.GetCustomAttributes(attributeType, false);
            var methodAttributes =
                (ITransactionAttributeSettings[])
                methodInfo.GetCustomAttributes(attributeType, false);
            if (classAttributes.Length == 0 && methodAttributes.Length == 0) {
                invocation.Proceed();
            }
            else {
                TransactionAttributeSettings transactionAttributeSettings =
                    GetTransactionAttributeSettings(methodAttributes, classAttributes);

                object transactionState = OnEntry(transactionAttributeSettings, null);
                try {
                    invocation.Proceed();
                }
                catch (Exception err) {
                    CloseUnitOfWork(transactionAttributeSettings, transactionState, err);
                    if (!(err is AbortTransactionException)) {
                        exceptionLogger.LogException(err, transactionAttributeSettings.IsExceptionSilent,
                                                     methodInfo.ReflectedType);
                    }
                    if (transactionManager.TransactionDepth == 0 &&
                        (transactionAttributeSettings.IsExceptionSilent || err is AbortTransactionException)) {
                        invocation.ReturnValue = transactionAttributeSettings.ReturnValue;
                        return;
                    }
                    throw;
                }
                transactionState = OnSuccess(transactionAttributeSettings, transactionState);
            }
        }

        #endregion

        protected virtual Type GetAttributeType() {
            return typeof(TransactionAttribute);
        }

        private TransactionAttributeSettings GetTransactionAttributeSettings(
            ITransactionAttributeSettings[] methodAttributes, ITransactionAttributeSettings[] classAttributes) {
            var transactionAttributeSettings = new TransactionAttributeSettings();
            if (methodAttributes.Length > 0) {
                transactionAttributeSettings = methodAttributes[methodAttributes.Length - 1].Settings;
            }
            else if (classAttributes.Length > 0) {
                transactionAttributeSettings = classAttributes[classAttributes.Length - 1].Settings;
            }
            return transactionAttributeSettings;
        }

        private object OnEntry(TransactionAttributeSettings transactionAttributeSettings, object transactionState) {
            return transactionManager.PushTransaction(transactionAttributeSettings.FactoryKey, transactionState);
        }


        private object OnSuccess(TransactionAttributeSettings transactionAttributeSettings, object transactionState) {
            return CloseUnitOfWork(transactionAttributeSettings, transactionState, null);
        }

        protected virtual object CloseUnitOfWork(TransactionAttributeSettings transactionAttributeSettings,
                                                 object transactionState, Exception err) {
            string factoryKey = transactionAttributeSettings.FactoryKey;
            if (err == null) {
                try {
                    NHibernateSession.CurrentFor(factoryKey).Flush();
                    transactionState = transactionManager.CommitTransaction(factoryKey, transactionState);
                }
                catch (Exception) {
                    transactionState = transactionManager.RollbackTransaction(factoryKey, transactionState);
                    transactionState = transactionManager.PopTransaction(factoryKey, transactionState);
                    throw;
                }

            }
            else {
                transactionState = transactionManager.RollbackTransaction(factoryKey, transactionState);
            }
            transactionState = transactionManager.PopTransaction(factoryKey, transactionState);

            return transactionState;
        }
    }
}