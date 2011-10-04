namespace SharpArch.RavenDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Raven.Client;

    using SharpArch.Domain.DomainModel;
    using SharpArch.Domain.PersistenceSupport;
    using SharpArch.RavenDb.Contracts.Repositories;

    public class RavenDbRepositoryWithTypedId<T, TIdT> : IRavenDbRepositoryWithTypedId<T, TIdT> where T : EntityWithTypedId<TIdT>
    {
        #region Constants and Fields

        private readonly IDocumentSession context;

        #endregion

        #region Constructors and Destructors

        public RavenDbRepositoryWithTypedId(IDocumentSession context)
        {
            this.context = context;
        }

        #endregion

        #region Implemented Interfaces

        #region IRavenDbRepositoryWithTypedId<T,TIdT>

        public IEnumerable<T> FindAll(Func<T, bool> where)
        {
            return this.context.Query<T>().Where(where);
        }

        public T FindOne(Func<T, bool> where)
        {
            IEnumerable<T> foundList = this.context.Query<T>().Where(where);

            if (foundList.Count() > 1)
            {
                throw new InvalidOperationException("The query returned more than one result. Please refine your query.");
            }

            if (foundList.Count() == 1)
            {
                return foundList.First();
            }

            return default(T);
        }

        public T First(Func<T, bool> where)
        {
            return this.context.Query<T>().First(where);
        }

        #endregion

        #region IRepositoryWithTypedId<T,TIdT>

        protected IDocumentSession Session
        {
            get
            {
                return this.context;
            }
        }

        public virtual IDbContext DbContext 
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Delete(T entity)
        {
            this.context.Delete(entity);
            this.context.SaveChanges();
        }

        public T Get(TIdT id)
        {
            return this.FindOne(w => Equals(w.Id, id));
        }

        public IList<T> GetAll()
        {
            return this.context.Query<T>().ToList();
        }

        public T SaveOrUpdate(T entity)
        {
            this.context.Store(entity);
            this.context.SaveChanges();

            return entity;
        }

        #endregion

        #endregion
    }
}