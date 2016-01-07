namespace SharpArch.RavenDb.Contracts.Repositories
{
    using SharpArch.Domain.PersistenceSupport;

    /// <summary>
    /// RavenDB repository for documents with primary key of type <see cref="int"/>.
    /// </summary>
    /// <typeparam name="T">The document type.</typeparam>
    /// <seealso cref="SharpArch.Domain.PersistenceSupport.IRepository{T}" />
    public interface IRavenDbRepository<T> : IRavenDbRepositoryWithTypedId<T, int>, IRepository<T>
    {
    }
}