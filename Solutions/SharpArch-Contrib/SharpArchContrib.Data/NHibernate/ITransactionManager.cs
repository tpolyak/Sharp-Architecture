namespace SharpArchContrib.Data.NHibernate {
    public interface ITransactionManager {
        int TransactionDepth { get; }
        string Name { get; }
        object PushTransaction(string factoryKey, object transactionState);
        bool TransactionIsActive(string factoryKey);
        object PopTransaction(string factoryKey, object transactionState);
        object RollbackTransaction(string factoryKey, object transactionState);
        object CommitTransaction(string factoryKey, object transactionState);
    }
}