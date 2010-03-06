using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpArch.Core.PersistenceSupport.NHibernate
{
    /// <summary>
    /// Extends the basic data repository interface with an interface that supports a number 
    /// of NHibernate specific methods while avoiding a concrete dependency on the NHibernate
    /// assembly.  For looser coupling, the "Core" layers of the SharpArch library and of your 
    /// application should not have a reference to the NHibernate assembly.
    /// </summary>
    public interface INHibernateRepository<T> : INHibernateRepositoryWithTypedId<T, int>, IRepository<T> { }

    public interface INHibernateRepositoryWithTypedId<T, IdT> : IRepositoryWithTypedId<T, IdT>
    {
        /// <summary>
        /// Returns null if a row is not found matching the provided Id.
        /// </summary>
        T Get(IdT id, Enums.LockMode lockMode);

        /// <summary>
        /// Throws an exception if a row is not found matching the provided Id.
        /// </summary>
        T Load(IdT id);

        /// <summary>
        /// Throws an exception if a row is not found matching the provided Id.
        /// </summary>
        T Load(IdT id, Enums.LockMode lockMode);

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
    }
}
