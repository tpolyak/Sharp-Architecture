﻿namespace SharpArch.Domain.PersistenceSupport
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DomainModel;
    using JetBrains.Annotations;


    /// <summary>
    ///     Defines the public members of a class that implements the asynchronous repository pattern for entities
    ///     of the specified type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TId">The type of the entity ID.</typeparam>
    [PublicAPI]
    public interface IRepository<TEntity, in TId>
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        ///     Returns the database context, which provides a handle to application wide DB
        ///     activities such as committing any pending changes, beginning a transaction,
        ///     rolling back a transaction, etc.
        /// </summary>
        
        ITransactionManager TransactionManager { get; }

        /// <summary>
        ///     Returns the entity that matches the specified ID.
        /// </summary>
        /// <remarks>
        ///     An entity or <c>null</c> if a row is not found matching the provided ID.
        /// </remarks>
        
        Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Returns all of the items of a given type.
        /// </summary>
        
        Task<IList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     For entities that have assigned Id's, you must explicitly call Save to add a new one.
        ///     See http://www.hibernate.org/hib_docs/nhibernate/html_single/#mapping-declaration-id-assigned.
        /// </summary>
        /// <returns>
        ///     Saved entity instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="entity" /> is <c>null</c>.</exception>
        
        Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Saves or updates the specified entity.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         For entities with automatically generated IDs, such as identity,
        ///         <see cref="SaveOrUpdateAsync(TEntity, CancellationToken)" />  may be called when saving or updating an entity.
        ///     </para>
        ///     <para>
        ///         Updating also allows you to commit changes to a detached object.
        ///         More info may be found at:
        ///         http://www.hibernate.org/hib_docs/nhibernate/html_single/#manipulatingdata-updating-detached
        ///     </para>
        /// </remarks>
        /// <returns>
        ///     Entity instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="entity" /> is <c>null</c>.</exception>
        
        Task<TEntity> SaveOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Disassociates the entity with the ORM so that changes made to it are not automatically
        ///     saved to the database.
        /// </summary>
        /// <remarks>
        ///     In NHibernate this removes the entity from current session cache.
        ///     More details may be found at http://www.hibernate.org/hib_docs/nhibernate/html_single/#performance-sessioncache.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="entity" /> is <c>null</c>.</exception>
        Task EvictAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes the specified entity.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="entity" /> is <c>null</c>.</exception>
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes the entity that matches the provided Id.
        /// </summary>
        Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
    }
}
