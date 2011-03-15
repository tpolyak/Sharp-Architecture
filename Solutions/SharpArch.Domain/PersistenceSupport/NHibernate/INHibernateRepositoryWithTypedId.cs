namespace SharpArch.Domain.PersistenceSupport.NHibernate
{
    using System.Collections.Generic;

    public interface INHibernateRepositoryWithTypedId<T, TId> : IRepositoryWithTypedId<T, TId>
    {
        /// <summary>
        /// Returns null if a row is not found matching the provided Id.
        /// </summary>
        T Get(TId id, Enums.LockMode lockMode);

        /// <summary>
        /// Throws an exception if a row is not found matching the provided Id.
        /// </summary>
        T Load(TId id);

        /// <summary>
        /// Throws an exception if a row is not found matching the provided Id.
        /// </summary>
        T Load(TId id, Enums.LockMode lockMode);

        /// <summary>
        /// Looks for zero or more instances using the <see cref="IDictionary{string, object}"/> provided.
        /// The key of the collection should be the property name and the value should be
        /// the value of the property to filter by.
        /// </summary>
        IList<T> FindAll(IDictionary<string, object> propertyValuePairs);

        /// <summary>
        /// Looks for a single instance using the property/values provided.
        /// </summary>
        /// <exception cref="NonUniqueResultException" />
        T FindOne(IDictionary<string, object> propertyValuePairs);

        /// <summary>
        /// Looks for zero or more instances using the example provided.
        /// </summary>
        IList<T> FindAll(T exampleInstance, params string[] propertiesToExclude);

        /// <summary>
        /// Looks for a single instance using the example provided.
        /// </summary>
        /// <exception cref="NonUniqueResultException" />
        T FindOne(T exampleInstance, params string[] propertiesToExclude);

        /// <summary>
        /// For entities that have assigned Id's, you must explicitly call Save to add a new one.
        /// See http://www.hibernate.org/hib_docs/nhibernate/html_single/#mapping-declaration-id-assigned.
        /// </summary>
        T Save(T entity);

        /// <summary>
        /// For entities that have assigned Id's, you should explicitly call Update to update an existing one.
        /// Updating also allows you to commit changes to a detached object.  More info may be found at:
        /// http://www.hibernate.org/hib_docs/nhibernate/html_single/#manipulatingdata-updating-detached
        /// </summary>
        T Update(T entity);

        /// <summary>
        /// Dissasociates the entity with the ORM so that changes made to it are not automatically 
        /// saved to the database.  More precisely, this removes the entity from <see cref="ISession" />'s cache.
        /// More details may be found at http://www.hibernate.org/hib_docs/nhibernate/html_single/#performance-sessioncache.
        /// </summary>
        void Evict(T entity);

        /// <summary>
        /// Provides a handle to application wide DB activities such as committing any pending changes,
        /// beginning a transaction, rolling back a transaction, etc.
        /// </summary>
        IDbContext DbContext { get; }
    }
}