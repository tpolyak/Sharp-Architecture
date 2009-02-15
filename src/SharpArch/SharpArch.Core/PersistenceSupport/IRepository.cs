using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpArch.Core.PersistenceSupport
{
    /// <summary>
    /// Provides a standard interface for DAOs which are data-access mechanism agnostic.
    /// 
    /// Since nearly all of the domain objects you create will have a type of int Id, this 
    /// base Idao leverages this assumption.  If you want an entity with a type 
    /// other than int, such as string, then use <see cref="IRepositoryWithTypedId{T, IdT}" />.
    /// </summary>
    public interface IRepository<T> : IRepositoryWithTypedId<T, int> { }

    public interface IRepositoryWithTypedId<T, IdT>
    {
        /// <summary>
        /// Returns null if a row is not found matching the provided Id.
        /// </summary>
        T Get(IdT id);

        /// <summary>
        /// Returns all of the items of a given type.
        /// </summary>
        List<T> GetAll();

        /// <summary>
        /// Looks for zero or more instances using the <see cref="IDictionary{string, object}"/> provided.
        /// The key of the collection should be the property name and the value should be
        /// the value of the property to filter by.
        /// </summary>
        List<T> FindAll(IDictionary<string, object> propertyValuePairs);

        /// <summary>
        /// Looks for a single instance using the property/values provided.
        /// </summary>
        /// <exception cref="NonUniqueResultException" />
        T FindOne(IDictionary<string, object> propertyValuePairs);

        /// <summary>
        /// For entities with automatatically generated Ids, such as identity, SaveOrUpdate may 
        /// be called when saving or updating an entity.
        /// 
        /// Updating also allows you to commit changes to a detached object.  More info may be found at:
        /// http://www.hibernate.org/hib_docs/nhibernate/html_single/#manipulatingdata-updating-detached
        /// </summary>
        T SaveOrUpdate(T entity);

        /// <summary>
        /// I'll let you guess what this does.
        /// </summary>
        void Delete(T entity);

        /// <summary>
        /// Provides a handle to application wide DB activities such as committing any pending changes,
        /// beginning a transaction, rolling back a transaction, etc.
        /// </summary>
        IDbContext DbContext { get; }
    }
}
