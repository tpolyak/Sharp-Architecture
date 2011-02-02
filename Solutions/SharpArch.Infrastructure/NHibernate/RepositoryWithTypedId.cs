namespace SharpArch.Infrastructure.NHibernate
{
    using System.Collections.Generic;

    using global::NHibernate;
    using global::NHibernate.Criterion;

    using SharpArch.Domain;
    using SharpArch.Domain.DomainModel;
    using SharpArch.Domain.PersistenceSupport;

    /// <summary>
    ///     Provides a fully loaded DAO which may be created in a few ways including:
    ///     * Direct instantiation; e.g., new RepositoryWithTypedId<Customer, string>
    ///                                                     * Spring configuration; e.g., <object id = "CustomerDao" type = "SharpArch.Data.NHibernateSupport.GenericDao&lt;CustomerAlias, string>, SharpArch.Data" autowire = "byName" />
    /// </summary>
    public class RepositoryWithTypedId<T, TId> : IRepositoryWithTypedId<T, TId>
    {
        private IDbContext dbContext;

        public virtual IDbContext DbContext
        {
            get
            {
                if (this.dbContext == null)
                {
                    var factoryKey = SessionFactoryAttribute.GetKeyFrom(this);
                    this.dbContext = new DbContext(factoryKey);
                }

                return this.dbContext;
            }
        }

        protected virtual ISession Session
        {
            get
            {
                var factoryKey = SessionFactoryAttribute.GetKeyFrom(this);
                return NHibernateSession.CurrentFor(factoryKey);
            }
        }

        public virtual void Delete(T entity)
        {
            this.Session.Delete(entity);
        }

        public virtual IList<T> FindAll(IDictionary<string, object> propertyValuePairs)
        {
            Check.Require(
                propertyValuePairs != null && propertyValuePairs.Count > 0, 
                "propertyValuePairs was null or empty; " + "it has to have at least one property/value pair in it");

            var criteria = this.Session.CreateCriteria(typeof(T));

            foreach (var key in propertyValuePairs.Keys)
            {
                criteria.Add(
                    propertyValuePairs[key] != null
                        ? Restrictions.Eq(key, propertyValuePairs[key])
                        : Restrictions.IsNull(key));
            }

            return criteria.List<T>();
        }

        public virtual T FindOne(IDictionary<string, object> propertyValuePairs)
        {
            var foundList = this.FindAll(propertyValuePairs);

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

        public virtual T Get(TId id)
        {
            return this.Session.Get<T>(id);
        }

        public virtual IList<T> GetAll()
        {
            var criteria = this.Session.CreateCriteria(typeof(T));
            return criteria.List<T>();
        }

        /// <summary>
        ///     Although SaveOrUpdate _can_ be invoked to update an object with an assigned Id, you are 
        ///     hereby forced instead to use Save/Update for better clarity.
        /// </summary>
        public virtual T SaveOrUpdate(T entity)
        {
            Check.Require(
                !(entity is IHasAssignedId<TId>), 
                "For better clarity and reliability, Entities with an assigned Id must call Save or Update");

            this.Session.SaveOrUpdate(entity);
            return entity;
        }
    }
}