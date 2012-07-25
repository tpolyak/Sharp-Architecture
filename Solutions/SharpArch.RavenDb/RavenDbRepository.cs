namespace SharpArch.RavenDb
{
    using Raven.Client;

    using SharpArch.Domain.DomainModel;
    using SharpArch.Domain.PersistenceSupport;
    using SharpArch.RavenDb.Contracts.Repositories;

    public class RavenDbRepository<T> : RavenDbRepositoryWithTypedId<T, string>,
        IRavenDbRepository<T>,
        ILinqRepository<T>
        where T : EntityWithTypedId<string>
    {
        public RavenDbRepository(IDocumentSession session) : base(session)
        {
        }

        public void Delete(int id)
        {
            base.Delete(id.ToString());
        }

        public T FindOne(int id)
        {
            return base.FindOne(id.ToString());
        }
    }
}