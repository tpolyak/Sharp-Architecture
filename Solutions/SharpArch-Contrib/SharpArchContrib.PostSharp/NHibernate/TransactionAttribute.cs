using System;
using Microsoft.Practices.ServiceLocation;
using PostSharp.Extensibility;
using PostSharp.Laos;
using SharpArch.Core;
using SharpArch.Data.NHibernate;
using SharpArchContrib.Core.Logging;
using SharpArchContrib.Data.NHibernate;

namespace SharpArchContrib.PostSharp.NHibernate {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    [MulticastAttributeUsage(MulticastTargets.Method, AllowMultiple = true)]
    [Serializable]
    public class TransactionAttribute : OnMethodBoundaryAspect, ITransactionAttributeSettings {
        private IExceptionLogger exceptionLogger;
        protected TransactionAttributeSettings settings;
        private ITransactionManager transactionManager;

        public TransactionAttribute() {
            settings = new TransactionAttributeSettings();
        }

        public string FactoryKey {
            get { return Settings.FactoryKey; }
            set {
                if (value == null) {
                    throw new PreconditionException("FactoryKey cannot be null");
                }
                Settings.FactoryKey = value;
            }
        }

        public bool IsExceptionSilent {
            get { return Settings.IsExceptionSilent; }
            set { Settings.IsExceptionSilent = value; }
        }

        public object ReturnValue {
            get { return Settings.ReturnValue; }
            set { Settings.ReturnValue = value; }
        }

        protected ITransactionManager TransactionManager {
            get {
                if (transactionManager == null) {
                    transactionManager = ServiceLocator.Current.GetInstance<ITransactionManager>();
                }
                return transactionManager;
            }
        }

        private IExceptionLogger ExceptionLogger {
            get {
                if (exceptionLogger == null) {
                    exceptionLogger = ServiceLocator.Current.GetInstance<IExceptionLogger>();
                }
                return exceptionLogger;
            }
        }

        #region ITransactionAttributeSettings Members

        public TransactionAttributeSettings Settings {
            get { return settings; }
            set {
                if (value == null) {
                    throw new PreconditionException("Settings must not be null");
                }
                settings = value;
            }
        }

        #endregion

        public override void OnEntry(MethodExecutionEventArgs eventArgs) {
            eventArgs.InstanceTag = TransactionManager.PushTransaction(FactoryKey, eventArgs.InstanceTag);
        }

        public override void OnException(MethodExecutionEventArgs eventArgs) {
            eventArgs.InstanceTag = CloseUnitOfWork(eventArgs);
            if (!(eventArgs.Exception is AbortTransactionException)) {
                ExceptionLogger.LogException(eventArgs.Exception, IsExceptionSilent, eventArgs.Method.DeclaringType);
            }
            if (TransactionManager.TransactionDepth == 0 &&
                (IsExceptionSilent || eventArgs.Exception is AbortTransactionException)) {
                eventArgs.FlowBehavior = FlowBehavior.Return;
                eventArgs.ReturnValue = ReturnValue;
            }
        }

        public override void OnSuccess(MethodExecutionEventArgs eventArgs) {
            eventArgs.InstanceTag = CloseUnitOfWork(eventArgs);
        }

        protected virtual object CloseUnitOfWork(MethodExecutionEventArgs eventArgs) {
            object transactionState = eventArgs.InstanceTag;
            if (eventArgs.Exception == null) {
                NHibernateSession.CurrentFor(FactoryKey).Flush();
                transactionState = TransactionManager.CommitTransaction(FactoryKey, transactionState);
            }
            else {
                transactionState = TransactionManager.RollbackTransaction(FactoryKey, transactionState);
            }
            transactionState = TransactionManager.PopTransaction(FactoryKey, transactionState);

            return transactionState;
        }
    }
}