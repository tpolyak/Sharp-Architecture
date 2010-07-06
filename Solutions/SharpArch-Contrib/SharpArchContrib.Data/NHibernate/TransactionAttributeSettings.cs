using System;
using SharpArch.Data.NHibernate;

namespace SharpArchContrib.Data.NHibernate {
    [Serializable]
    public class TransactionAttributeSettings {
        public TransactionAttributeSettings() {
            FactoryKey = NHibernateSession.DefaultFactoryKey;
            IsExceptionSilent = false;
            ReturnValue = null;
        }

        public string FactoryKey { get; set; }
        public bool IsExceptionSilent { get; set; }
        public object ReturnValue { get; set; }
    }
}