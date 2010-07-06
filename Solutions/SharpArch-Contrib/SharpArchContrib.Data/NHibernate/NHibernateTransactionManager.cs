using System;
using NHibernate;
using SharpArch.Data.NHibernate;

namespace SharpArchContrib.Data.NHibernate {
    /// <summary>
    /// Provides support for System.Transaction transactions
    /// </summary>
    [Serializable]
    public class NHibernateTransactionManager : TransactionManagerBase {
        public override string Name {
            get { return "NHibernate TransactionManager"; }
        }

        public override object PushTransaction(string factoryKey, object transactionState) {
            transactionState = base.PushTransaction(factoryKey, transactionState);

            ITransaction transaction = NHibernateSession.CurrentFor(factoryKey).Transaction;
            if (!transaction.IsActive) {
                transaction.Begin();
            }

            return transactionState;
        }

        public override bool TransactionIsActive(string factoryKey) {
            ITransaction transaction = NHibernateSession.CurrentFor(factoryKey).Transaction;
            return transaction != null && transaction.IsActive;
        }

        public override object RollbackTransaction(string factoryKey, object transactionState) {
            ITransaction transaction = NHibernateSession.CurrentFor(factoryKey).Transaction;
            if (TransactionDepth == 1 && transaction.IsActive) {
                transaction.Rollback();
            }

            return transactionState;
        }

        public override object CommitTransaction(string factoryKey, object transactionState) {
            ITransaction transaction = NHibernateSession.CurrentFor(factoryKey).Transaction;
            if (TransactionDepth == 1 && transaction.IsActive) {
                transaction.Commit();
            }

            return transactionState;
        }
    }
}