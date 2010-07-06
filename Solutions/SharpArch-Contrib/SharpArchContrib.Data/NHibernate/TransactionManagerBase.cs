using System;
using System.Threading;
using log4net;

namespace SharpArchContrib.Data.NHibernate {
    [Serializable]
    public abstract class TransactionManagerBase : ITransactionManager {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TransactionManagerBase));
        [ThreadStatic]
        private static int transactionDepth;

        #region ITransactionManager Members

        public int TransactionDepth {
            get { return transactionDepth; }
        }

        public virtual object PushTransaction(string factoryKey, object transactionState) {
            Interlocked.Increment(ref transactionDepth);
            Log(string.Format("Push Transaction to Depth {0}", transactionDepth));
            return transactionState;
        }

        public abstract bool TransactionIsActive(string factoryKey);

        public virtual object PopTransaction(string factoryKey, object transactionState) {
            Interlocked.Decrement(ref transactionDepth);
            Log(string.Format("Pop Transaction to Depth {0}", transactionDepth));
            return transactionState;
        }

        public abstract object RollbackTransaction(string factoryKey, object transactionState);
        public abstract object CommitTransaction(string factoryKey, object transactionState);
        public abstract string Name { get; }

        #endregion

        protected void Log(string message) {
            logger.Debug(string.Format("{0}: {1}", Name, message));
        }
    }
}