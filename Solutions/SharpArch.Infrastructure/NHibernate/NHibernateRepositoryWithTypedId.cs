namespace SharpArch.Infrastructure.NHibernate
{
    using System.Collections.Generic;
    using System.Reflection;

    using global::NHibernate;
    using global::NHibernate.Criterion;

    using SharpArch.Domain;
    using SharpArch.Domain.PersistenceSupport.NHibernate;

    /// <summary>
    ///     Provides a fully loaded DAO which may be created in a few ways including:
    ///     * Direct instantiation; e.g., new GenericDao<Customer, string>
    ///                                                     * Spring configuration; e.g., <object id = "CustomerDao" type = "SharpArch.Data.NHibernateSupport.GenericDao&lt;CustomerAlias, string>, SharpArch.Data" autowire = "byName" />
    /// </summary>
    public class NHibernateRepositoryWithTypedId<T, TId> : RepositoryWithTypedId<T, TId>, 
                                                           INHibernateRepositoryWithTypedId<T, TId>
    {
        public virtual void Evict(T entity)
        {
            this.Session.Evict(entity);
        }

        public virtual IList<T> FindAll(T exampleInstance, params string[] propertiesToExclude)
        {
            var criteria = this.Session.CreateCriteria(typeof(T));
            var example = Example.Create(exampleInstance);

            foreach (var propertyToExclude in propertiesToExclude)
            {
                example.ExcludeProperty(propertyToExclude);
            }

            criteria.Add(example);

            return criteria.List<T>();
        }

        public virtual T FindOne(T exampleInstance, params string[] propertiesToExclude)
        {
            var foundList = this.FindAll(exampleInstance, propertiesToExclude);

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

        /// <summary>
        ///     Translates a domain layer lock mode into an NHibernate lock mode via reflection.  This is 
        ///     provided to facilitate developing the domain layer without a direct dependency on the 
        ///     NHibernate assembly.
        /// </summary>
        private static LockMode ConvertFrom(Enums.LockMode lockMode)
        {
            var translatedLockMode = typeof(LockMode).GetField(
                lockMode.ToString(), BindingFlags.Public | BindingFlags.Static);

            Check.Ensure(
                translatedLockMode != null, 
                "The provided lock mode , '" + lockMode + ",' " +
                "could not be translated into an NHibernate.LockMode. This is probably because " +
                "NHibernate was updated and now has different lock modes which are out of synch " +
                "with the lock modes maintained in the domain layer.");

            return (LockMode)translatedLockMode.GetValue(null);
        }
    }
}