namespace SharpArch.NHibernate
{
    using System.Linq;
    using global::NHibernate;
    using global::NHibernate.Linq;

    using Domain.PersistenceSupport;
    using Domain.Specifications;

    public class LinqRepositoryWithTypedId<T, TId> : NHibernateRepositoryWithTypedId<T, TId>, ILinqRepositoryWithTypedId<T, TId>
    {
        public LinqRepositoryWithTypedId(ITransactionManager transactionManager, ISession session) : base(transactionManager, session)
        {
        }

        public IQueryable<T> FindAll()
        {
            return this.Session.Query<T>();
        }

        public IQueryable<T> FindAll(ILinqSpecification<T> specification)
        {
            return specification.SatisfyingElementsFrom(this.Session.Query<T>());
        }

        public T FindOne(TId id)
        {
            return this.Session.Get<T>(id);
        }

        public T FindOne(ILinqSpecification<T> specification)
        {
            return specification.SatisfyingElementsFrom(this.Session.Query<T>()).SingleOrDefault();
        }

    }
}