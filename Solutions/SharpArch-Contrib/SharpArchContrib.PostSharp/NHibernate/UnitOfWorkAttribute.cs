using System;
using PostSharp.Extensibility;
using PostSharp.Laos;
using SharpArch.Data.NHibernate;
using SharpArchContrib.Data.NHibernate;

namespace SharpArchContrib.PostSharp.NHibernate {
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    [MulticastAttributeUsage(MulticastTargets.Method, AllowMultiple = true)]
    public sealed class UnitOfWorkAttribute : TransactionAttribute {
        public UnitOfWorkAttribute() {
            settings = new UnitOfWorkAttributeSettings();
        }

        public bool CloseSessions {
            get { return UnitOfWorkSettings.CloseSessions; }
            set { UnitOfWorkSettings.CloseSessions = value; }
        }

        public UnitOfWorkAttributeSettings UnitOfWorkSettings {
            get { return (UnitOfWorkAttributeSettings) settings; }
            set { Settings = value; }
        }

        protected override object CloseUnitOfWork(MethodExecutionEventArgs eventArgs) {
            object transactionState = base.CloseUnitOfWork(eventArgs);
            if (TransactionManager.TransactionDepth == 0) {
                var sessionStorage = (NHibernateSession.Storage as IUnitOfWorkSessionStorage);
                if (sessionStorage != null) {
                    sessionStorage.EndUnitOfWork(CloseSessions);
                }
            }

            return transactionState;
        }
    }
}