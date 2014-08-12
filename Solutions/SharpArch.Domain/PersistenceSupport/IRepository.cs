namespace SharpArch.Domain.PersistenceSupport
{
    /// <summary>
    ///     Provides a standard interface for DAOs which are data-access mechanism agnostic.
    /// 
    ///     Since nearly all of the domain objects you create will have a type of int Id, this 
    ///     base Idao leverages this assumption.  If you want an entity with a type 
    ///     other than int, such as string, then use <see cref = "IRepositoryWithTypedId{T, IdT}" />.
    /// </summary>
    public interface IRepository<T> : IRepositoryWithTypedId<T, int>
    {
    }
}