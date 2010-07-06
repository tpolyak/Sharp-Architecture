using System;
using SharpArch.Core;
using SharpArchContrib.Data.NHibernate;

namespace SharpArchContrib.Castle.NHibernate {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class UnitOfWorkAttribute : Attribute, ITransactionAttributeSettings {
        private TransactionAttributeSettings settings;

        public UnitOfWorkAttribute() {
            settings = new UnitOfWorkAttributeSettings();
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

        public bool CloseSessions {
            get { return UnitOfWorkSettings.CloseSessions; }
            set { UnitOfWorkSettings.CloseSessions = value; }
        }

        public UnitOfWorkAttributeSettings UnitOfWorkSettings {
            get { return (UnitOfWorkAttributeSettings) settings; }
            set { Settings = value; }
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
    }
}