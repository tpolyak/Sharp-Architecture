namespace SharpArch.Domain.PersistenceSupport
{
    using System.Collections.Generic;

    /// <summary>
    ///     Defines the public members of a class that implements the repository pattern for entities
    ///     of the specified type.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="TId">The type of the entity ID.</typeparam>
    public interface IRepositoryWithTypedId<T, TId>
    {
        /// <summary>
        ///     Returns the database context, which provides a handle to application wide DB
        ///     activities such as committing any pending changes, beginning a transaction,
        ///     rolling back a transaction, etc.
        /// </summary>
        IDbContext DbContext { get; }

        /// <summary>
        ///     Returns the entity that matches the specified ID.
        /// </summary>
        /// <remarks>
        ///     An entity or <c>null</c> if a row is not found matching the provided ID.
        /// </remarks>
        T Get(TId id);

        /// <summary>
        ///     Returns all of the items of a given type.
        /// </summary>
        IList<T> GetAll();

        /// <summary>
        ///     Saves or updates the specified entity.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         For entities with automatically generated IDs, such as identity,
        ///         <see cref="SaveOrUpdate"/>  may be called when saving or updating an entity.
        ///     </para>
        ///     <para>
        ///         Updating also allows you to commit changes to a detached object.
        ///         More info may be found at:
        ///         http://www.hibernate.org/hib_docs/nhibernate/html_single/#manipulatingdata-updating-detached
        ///     </para>
        /// </remarks>
        T SaveOrUpdate(T entity);

        /// <summary>
        ///     Deletes the specified entity.
        /// </summary>
        void Delete(T entity);

        /// <summary>
        ///     Deletes the entity that matches the provided ID.
        /// </summary>
        void Delete(TId id);
    }
}