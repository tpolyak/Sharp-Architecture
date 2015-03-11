namespace SharpArch.Domain.PersistenceSupport
{
    using System.Linq;

    using Specifications;

    /// <summary>
    ///     Defines the public members of a LINQ supported repository.
    /// </summary>
    /// <remarks>
    ///     Defines a LINQ implementation of the Repository Pattern that takes in a Specification to
    ///     define the items that should be returned.
    /// </remarks>
    public interface ILinqRepositoryWithTypedId<T, TId>
    {
        /// <summary>
        ///     Deletes the specified object from the repository
        /// </summary>
        /// <param name="target">The entity.</param>
        void Delete(T target);

        /// <summary>
        ///     Deletes the object that matches the supplied ID from the repository
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        void Delete(TId id);

        /// <summary>
        ///     Saves the specified object to the repository
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Save(T entity);

        /// <summary>
        ///     Saves the specified object to the repository and evicts it from the session.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void SaveAndEvict(T entity);

        /// <summary>
        ///     Finds an item by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The matching item.</returns>
        T FindOne(TId id);

        /// <summary>
        ///     Finds an item by a specification
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The matching item.</returns>
        T FindOne(ILinqSpecification<T> specification);

        /// <summary>
        ///     Finds all items within the repository.
        /// </summary>
        /// <returns>All items in the repository.</returns>
        IQueryable<T> FindAll();

        /// <summary>
        ///     Finds all items by a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>All matching items.</returns>
        IQueryable<T> FindAll(ILinqSpecification<T> specification);
    }
}