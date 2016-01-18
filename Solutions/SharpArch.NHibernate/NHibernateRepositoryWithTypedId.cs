using System;

namespace SharpArch.NHibernate
{
    using System.Collections.Generic;
    using Contracts.Repositories;

    using Domain;
    using Domain.DomainModel;
    using Domain.PersistenceSupport;
    
    using global::NHibernate;
    using global::NHibernate.Criterion;

    /// <summary>
    ///     Provides a fully loaded DAO which may be created in a few ways including:
    ///     * Direct instantiation; e.g., new GenericDao&lt;Customer, string&gt;
    ///     * Spring configuration; e.g., <object id = "CustomerDao" type = "SharpArch.Data.NHibernateSupport.GenericDao&lt;CustomerAlias, string>, SharpArch.Data" autowire = "byName" />
    /// </summary>
    public class NHibernateRepositoryWithTypedId<T, TId> : INHibernateRepositoryWithTypedId<T, TId>
    {
        #region Constants and Fields

        private readonly ITransactionManager transactionManager;
        private readonly ISession session;

        #endregion

        public NHibernateRepositoryWithTypedId(ITransactionManager transactionManager, ISession session)
        {
            Check.Require(transactionManager != null, "TransactionManager is required.");
            Check.Require(session != null, "Session is required.");

            this.transactionManager = transactionManager;
            this.session = session;
        }

        #region Properties

        public virtual ITransactionManager TransactionManager
        {
            get
            {
                return this.transactionManager;
            }
        }

        protected virtual ISession Session
        {
            get { return this.session; }
        }

        #endregion

        #region Implemented Interfaces

        #region INHibernateRepositoryWithTypedId<T,TId>

        public virtual void Evict(T entity)
        {
            this.Session.Evict(entity);
        }

        public virtual IList<T> FindAll(T exampleInstance, params string[] propertiesToExclude)
        {
            ICriteria criteria = this.Session.CreateCriteria(typeof(T));
            Example example = Example.Create(exampleInstance);

            foreach (string propertyToExclude in propertiesToExclude)
            {
                example.ExcludeProperty(propertyToExclude);
            }

            criteria.Add(example);

            return criteria.List<T>();
        }

        public virtual IList<T> FindAll(IDictionary<string, object> propertyValuePairs)
        {
            Check.Require(propertyValuePairs != null && propertyValuePairs.Count > 0, "propertyValuePairs was null or empty; " + "it has to have at least one property/value pair in it");

            ICriteria criteria = this.Session.CreateCriteria(typeof(T));

            foreach (string key in propertyValuePairs.Keys)
            {
                criteria.Add(propertyValuePairs[key] != null ? Restrictions.Eq(key, propertyValuePairs[key]) : Restrictions.IsNull(key));
            }

            return criteria.List<T>();
        }

        public virtual T FindOne(T exampleInstance, params string[] propertiesToExclude)
        {
            IList<T> foundList = this.FindAll(exampleInstance, propertiesToExclude);

            if (foundList.Count > 1)
            {
                throw new NonUniqueResultException(foundList.Count);
            }

            if (foundList.Count == 1)
            {
                return foundList[0];
            }

            return default(T);
        }

        public virtual T FindOne(IDictionary<string, object> propertyValuePairs)
        {
            IList<T> foundList = this.FindAll(propertyValuePairs);

            if (foundList.Count > 1)
            {
                throw new NonUniqueResultException(foundList.Count);
            }

            if (foundList.Count == 1)
            {
                return foundList[0];
            }

            return default(T);
        }

        public virtual T Get(TId id, Enums.LockMode lockMode)
        {
            return this.Session.Get<T>(id, ConvertFrom(lockMode));
        }

        public virtual T Load(TId id)
        {
            return this.Session.Load<T>(id);
        }

        public virtual T Load(TId id, Enums.LockMode lockMode)
        {
            return this.Session.Load<T>(id, ConvertFrom(lockMode));
        }

        public virtual T Save(T entity)
        {
            this.Session.Save(entity);
            return entity;
        }

        
        public virtual T Update(T entity)
        {
            this.Session.Update(entity);
            return entity;
        }

        #endregion

        #region IRepositoryWithTypedId<T,TId>

        public virtual void Delete(T entity)
        {
            this.Session.Delete(entity);
        }

        public void Delete(TId id)
        {
            T entity = this.Get(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public virtual T Get(TId id)
        {
            return this.Session.Get<T>(id);
        }

        public virtual IList<T> GetAll()
        {
            ICriteria criteria = this.Session.CreateCriteria(typeof(T));
            return criteria.List<T>();
        }

        /// <summary>
        ///     Although SaveOrUpdate _can_ be invoked to update an object with an assigned Id, you are 
        ///     hereby forced instead to use Save/Update for better clarity.
        /// </summary>
        public virtual T SaveOrUpdate(T entity)
        {
            Check.Require(!(entity is IHasAssignedId<TId>), "For better clarity and reliability, Entities with an assigned Id must call Save or Update");

            this.Session.SaveOrUpdate(entity);
            return entity;
        }

        #endregion

        #endregion

        #region Methods


        /// <summary>
        ///     Translates a domain layer lock mode into an NHibernate lock mode via reflection.  This is 
        ///     provided to facilitate developing the domain layer without a direct dependency on the 
        ///     NHibernate assembly.
        /// </summary>
        private static LockMode ConvertFrom(Enums.LockMode lockMode)
        {
            switch (lockMode)
            {
                case Enums.LockMode.None:
                    return LockMode.None;
                case Enums.LockMode.Read:
                    return LockMode.Read;
                case Enums.LockMode.Upgrade:
                    return LockMode.Upgrade;
                case Enums.LockMode.UpgradeNoWait:
                    return LockMode.UpgradeNoWait;
                case Enums.LockMode.Write:
                    return LockMode.Write;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lockMode), lockMode,
                        "The provided lock mode , '" + lockMode + ",' could not be translated into an NHibernate.LockMode. " +
                        "This is probably because NHibernate was updated and now has different lock modes which are out of synch " +
                        "with the lock modes maintained in the domain layer.");
            }
        }

        #endregion
    }
}