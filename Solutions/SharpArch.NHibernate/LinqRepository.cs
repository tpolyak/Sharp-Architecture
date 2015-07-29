namespace SharpArch.NHibernate
{
    using global::NHibernate;

    using Domain.PersistenceSupport;

    public class LinqRepository<T> : LinqRepositoryWithTypedId<T, int>, ILinqRepository<T>
    {
        public LinqRepository(ITransactionManager transactionManager, ISession session) : base(transactionManager, session)
        {
        }
    }
}