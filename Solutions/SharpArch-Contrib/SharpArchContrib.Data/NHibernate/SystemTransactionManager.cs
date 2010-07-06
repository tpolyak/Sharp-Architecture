using System;
using System.Transactions;
using NHibernate;
using SharpArch.Data.NHibernate;

namespace SharpArchContrib.Data.NHibernate {
    /// <summary>
    /// Provides support for System.Transaction transactions
    /// </summary>
    [Serializable]
    public class SystemTransactionManager : TransactionManagerBase {
        public override string Name {
            get { return "System TransactionManager"; }
        }

        public override object PushTransaction(string factoryKey, object transactionState) {
            transactionState = base.PushTransaction(factoryKey, transactionState);

            //If this is a new transaction, we have to close the session,
            //start the transaction and then open the new session to 
            //associated the NHibernate session with the transaction.
            //This is usually not a high cost activity since the connection
            //will be pulled out of the connection pool
            ISession session = NHibernateSession.CurrentFor(factoryKey);
            bool newTransaction = !TransactionIsActive(factoryKey);
            if (newTransaction) {
                session.Disconnect();
            }
            transactionState = new TransactionScope();

            if (newTransaction) {
                session.Reconnect();
            }

            return transactionState;
        }

        public override bool TransactionIsActive(string factoryKey) {
            return Transaction.Current != null;
        }

        public override object PopTransaction(string factoryKey, object transactionState) {
            var transactionScope = (transactionState as TransactionScope);
            if (transactionScope != null) {
                transactionScope.Dispose();
                transactionState = null;
            }

            return base.PopTransaction(factoryKey, transactionState);
        }

        public override object RollbackTransaction(string factoryKey, object transactionState) {
            return transactionState;
        }

        public override object CommitTransaction(string factoryKey, object transactionState) {
            var transactionScope = (transactionState as TransactionScope);
            if (transactionScope != null) {
                transactionScope.Complete();
            }

            return transactionState;
        }
    }
}