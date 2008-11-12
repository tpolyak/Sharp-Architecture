using System.Collections.Generic;
using NHibernate;
using System.Reflection;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using NHibernate.Criterion;
using System.Collections.Specialized;
using System;
using SharpArch.Core.PersistenceSupport.NHibernate;

namespace SharpArch.Data.NHibernate
{
    /// <summary>
    /// Since nearly all of the domain objects you create will have a type of int ID, this 
    /// most freqently used base GenericDao leverages this assumption.  If you want a persistent 
    /// object with a type other than int, such as string, then use 
    /// <see cref="GenericDaoWithTypedId{T, IdT}" />.
    /// </summary>
    public class Repository<T> : RepositoryWithTypedId<T, int>, INHibernateRepository<T> { }

    /// <summary>
    /// Provides a fully loaded DAO which may be created in a few ways including:
    /// * Direct instantiation; e.g., new GenericDao<Customer, string>
    /// * Spring configuration; e.g., <object id="CustomerDao" type="SharpArch.Data.NHibernateSupport.GenericDao&lt;CustomerAlias, string>, SharpArch.Data" autowire="byName" />
    /// </summary>
    public class RepositoryWithTypedId<T, IdT> : INHibernateRepositoryWithTypedId<T, IdT>
    {
        protected ISession Session {
            get { return NHibernateSession.Current; }
        }

        public IDbContext DbContext {
            get {
                return SharpArch.Data.NHibernate.DbContext.Instance;
            }
        }

        public T Get(IdT id) {
            return Session.Get<T>(id);
        }

        public T Get(IdT id, Enums.LockMode lockMode) {
            return Session.Get<T>(id, ConvertFrom(lockMode));
        }

        public T Load(IdT id) {
            return Session.Load<T>(id);
        }

        public T Load(IdT id, Enums.LockMode lockMode) {
            return Session.Load<T>(id, ConvertFrom(lockMode));
        }

        public List<T> GetAll() {
            ICriteria criteria = Session.CreateCriteria(typeof(T));
            return criteria.List<T>() as List<T>;
        }

        public List<T> GetByExample(T exampleInstance, params string[] propertiesToExclude) {
            ICriteria criteria = Session.CreateCriteria(typeof(T));
            Example example = Example.Create(exampleInstance);

            foreach (string propertyToExclude in propertiesToExclude) {
                example.ExcludeProperty(propertyToExclude);
            }

            criteria.Add(example);

            return criteria.List<T>() as List<T>;
        }

        public T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude) {
            List<T> foundList = GetByExample(exampleInstance, propertiesToExclude);

            if (foundList.Count > 1) {
                throw new NonUniqueResultException(foundList.Count);
            }
            else if (foundList.Count == 1) {
                return foundList[0];
            }

            return default(T);
        }

        public List<T> GetByProperties(IDictionary<string, object> propertyValuePairs) {
            Check.Require(propertyValuePairs != null && propertyValuePairs.Count > 0,
                "propertyValuePairs was null or empty; " +
                "it has to have at least one property/value pair in it");

            ICriteria criteria = Session.CreateCriteria(typeof(T));

            foreach (string key in propertyValuePairs.Keys) {
                criteria.Add(Expression.Eq(key, propertyValuePairs[key]));
            }

            return criteria.List<T>() as List<T>;
        }

        public T GetUniqueByProperties(IDictionary<string, object> propertyValuePairs) {
            List<T> foundList = GetByProperties(propertyValuePairs);

            if (foundList.Count > 1) {
                throw new NonUniqueResultException(foundList.Count);
            }
            else if (foundList.Count == 1) {
                return foundList[0];
            }

            return default(T);
        }

        public T Save(T entity) {
            Session.Save(entity);
            return entity;
        }

        public T Update(T entity) {
            Session.Update(entity);
            return entity;
        }

        public void Delete(T entity) {
            Session.Delete(entity);
        }

        public void Evict(T entity) {
            Session.Evict(entity);
        }

        /// <summary>
        /// Although SaveOrUpdate _can_ be invoked to update an object with an assigned ID, you are 
        /// hereby forced instead to use Save/Update for better clarity.
        /// </summary>
        public T SaveOrUpdate(T entity) {
            Check.Require(!(entity is IHasAssignedId<IdT>),
                "For better clarity and reliability, PersistentObjects with an assigned ID must call Save or Update");

            Session.SaveOrUpdate(entity);
            return entity;
        }

        /// <summary>
        /// Translates a domain layer lock mode into an NHibernate lock mode via reflection.  This is 
        /// provided to facilitate developing the domain layer without a direct dependency on the 
        /// NHibernate assembly.
        /// </summary>
        private LockMode ConvertFrom(Enums.LockMode lockMode) {
            FieldInfo translatedLockMode = typeof(LockMode).GetField(lockMode.ToString(),
                BindingFlags.Public | BindingFlags.Static);

            Check.Ensure(translatedLockMode != null, "The provided lock mode , '" + lockMode + ",' " +
                    "could not be translated into an NHibernate.LockMode. This is probably because " +
                    "NHibernate was updated and now has different lock modes which are out of synch " +
                    "with the lock modes maintained in the domain layer.");

            return (LockMode)translatedLockMode.GetValue(null);
        }
    }
}
