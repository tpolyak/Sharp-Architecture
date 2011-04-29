namespace SharpArch.RavenDb.Contracts.Repositories
{
    using SharpArch.Domain.PersistenceSupport;

    public interface IRavenDbRepository<T> : IRavenDbRepositoryWithTypeId<T, int>, IRepository<T>
    {
    }
}