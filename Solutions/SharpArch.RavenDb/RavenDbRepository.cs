namespace SharpArch.RavenDb
{
    using Raven.Client;

    using SharpArch.Domain.DomainModel;
    using SharpArch.RavenDb.Contracts.Repositories;

    public class RavenDbRepository<T> : RavenDbRepositoryWithTypeId<T, int>, IRavenDbRepository<T> where T : Entity
    {
        public RavenDbRepository(IDocumentSession context) : base(context)
        {
        }
    }
}