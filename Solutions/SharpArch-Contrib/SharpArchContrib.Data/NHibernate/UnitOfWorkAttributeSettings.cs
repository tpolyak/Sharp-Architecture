using System;

namespace SharpArchContrib.Data.NHibernate {
    [Serializable]
    public class UnitOfWorkAttributeSettings : TransactionAttributeSettings {
        public UnitOfWorkAttributeSettings() {
            CloseSessions = true;
        }

        public bool CloseSessions { get; set; }
    }
}