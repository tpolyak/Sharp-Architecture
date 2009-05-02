using System.Collections.Generic;
using NHibernate;
using System.Reflection;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using NHibernate.Criterion;
using System.Collections.Specialized;
using System;
using SharpArch.Core.PersistenceSupport.NHibernate;
using SharpArch.Core.DomainModel;

namespace SharpArch.Data.NHibernate
{
    /// <summary>
    /// Since nearly all of the domain objects you create will have a type of int Id, this 
    /// most freqently used base GenericDao leverages this assumption.  If you want an entity
    /// with a type other than int, such as string, then use 
    /// <see cref="GenericDaoWithTypedId{T, IdT}" />.
    /// </summary>
    public class Repository<T> : RepositoryWithTypedId<T, int>, IRepository<T> { }

    /// <summary>
    /// Provides a fully loaded DAO which may be created in a few ways including:
    /// * Direct instantiation; e.g., new GenericDao<Customer, string>
    /// * Spring configuration; e.g., <object id="CustomerDao" type="SharpArch.Data.NHibernateSupport.GenericDao&lt;CustomerAlias, string>, SharpArch.Data" autowire="byName" />
    /// </summary>
    public class RepositoryWithTypedId<T, IdT> : IRepositoryWithTypedId<T, IdT>
    {
        protected virtual ISession Session {
            get {
                string factoryKey = SessionFactoryAttribute.GetKeyFrom(this);
                return NHibernateSession.CurrentFor(factoryKey);
            }
        }

        public virtual IDbContext DbContext {
            get {
                if (dbContext == null) {
                    string factoryKey = SessionFactoryAttribute.GetKeyFrom(this);
                    dbContext = new DbContext(factoryKey);
                }

                return dbContext;
            }
        }

        public virtual T Get(IdT id) {
            return Session.Get<T>(id);
        }

        public virtual IList<T> GetAll() {
            ICriteria criteria = Session.CreateCriteria(typeof(T));
            return criteria.List<T>();
        }

        public virtual IList<T> FindAll(IDictionary<string, object> propertyValuePairs) {
            Check.Require(propertyValuePairs != null && propertyValuePairs.Count > 0,
                "propertyValuePairs was null or empty; " +
                "it has to have at least one property/value pair in it");

            ICriteria criteria = Session.CreateCriteria(typeof(T));

            foreach (string key in propertyValuePairs.Keys) {
                if (propertyValuePairs[key] != null) {
                    criteria.Add(Expression.Eq(key, propertyValuePairs[key]));
                }
                else {
                    criteria.Add(Expression.IsNull(key));
                }
            }

            return criteria.List<T>();
        }

        public virtual T FindOne(IDictionary<string, object> propertyValuePairs) {
            IList<T> foundList = FindAll(propertyValuePairs);

            if (foundList.Count > 1) {
                throw new NonUniqueResultException(foundList.Count);
            }
            else if (foundList.Count == 1) {
                return foundList[0];
            }

            return default(T);
        }

        public virtual void Delete(T entity) {
            Session.Delete(entity);
        }

        /// <summary>
        /// Although SaveOrUpdate _can_ be invoked to update an object with an assigned Id, you are 
        /// hereby forced instead to use Save/Update for better clarity.
        /// </summary>
        public virtual T SaveOrUpdate(T entity) {
            Check.Require(!(entity is IHasAssignedId<IdT>),
                "For better clarity and reliability, Entities with an assigned Id must call Save or Update");

            Session.SaveOrUpdate(entity);
            return entity;
        }

        private IDbContext dbContext;
    }
}
