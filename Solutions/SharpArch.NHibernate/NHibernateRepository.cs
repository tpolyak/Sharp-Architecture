namespace SharpArch.NHibernate
{
    using global::NHibernate;

    using Domain.PersistenceSupport;
    using Contracts.Repositories;

    /// <summary>
    ///     Since nearly all of the domain objects you create will have a type of int Id, this 
    ///     most freqently used base NHibernateRepository leverages this assumption.  If you want 
    ///     an entity with a type other than int, such as string, then use 
    ///     <see cref = "NHibernateRepositoryWithTypedId{T, IdT}" />.
    /// </summary>
    public class NHibernateRepository<T> : NHibernateRepositoryWithTypedId<T, int>, INHibernateRepository<T>
    {
        public NHibernateRepository(ITransactionManager transactionManager, ISession session) : base(transactionManager, session)
        {
        }
    }
}