using System;
using SharpArch.Core;
using SharpArchContrib.Data.NHibernate;

namespace SharpArchContrib.Castle.NHibernate {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class TransactionAttribute : Attribute, ITransactionAttributeSettings {
        private TransactionAttributeSettings settings;

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