namespace SharpArch.Domain.PersistenceSupport
{
    /// <summary>
    ///     Defines the public members of a LINQ supported repository.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public interface ILinqRepository<T> : ILinqRepositoryWithTypedId<T, int>
    {
    }
}