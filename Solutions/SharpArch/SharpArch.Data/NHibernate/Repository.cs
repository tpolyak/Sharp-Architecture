namespace SharpArch.Data.NHibernate
{
    using SharpArch.Core.PersistenceSupport;

    /// <summary>
    ///     Since nearly all of the domain objects you create will have a type of int Id, this 
    ///     most freqently used base Repository leverages this assumption.  If you want an entity
    ///     with a type other than int, such as string, then use 
    ///     <see cref = "RepositoryWithTypedId{T, TId}" />.
    /// </summary>
    public class Repository<T> : RepositoryWithTypedId<T, int>, IRepository<T>
    {
    }
}