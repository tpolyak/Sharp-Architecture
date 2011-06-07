namespace SharpArch.RavenDb
{
    using System;

    using Raven.Client;

    using SharpArch.Domain.DomainModel;
    using SharpArch.Domain.PersistenceSupport;
    using SharpArch.RavenDb.Contracts.Repositories;

    public class RavenDbRepository<T> : RavenDbRepositoryWithTypeId<T, int>, IRavenDbRepository<T> where T : Entity
    {
        public RavenDbRepository(IDocumentSession context) : base(context)
        {
        }

        public virtual IDbContext DbContext
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}