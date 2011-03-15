namespace SharpArch.Domain.PersistenceSupport.RavenDb
{
    public interface IRavenDbRepository<T> : IRavenDbRepositoryWithTypeId<T, int>, IRepository<T>
    {
    }
}