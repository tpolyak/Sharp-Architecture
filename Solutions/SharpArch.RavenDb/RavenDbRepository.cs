namespace SharpArch.RavenDb
{
    using System;

    using Raven.Client;

    using SharpArch.Domain.DomainModel;
    using SharpArch.Domain.PersistenceSupport;
    using SharpArch.RavenDb.Contracts.Repositories;

    public class RavenDbRepository<T> : RavenDbRepositoryWithTypedId<T, string>, IRavenDbRepository<T> where T : EntityWithTypedId<string>
    {
        public RavenDbRepository(IDocumentSession session) : base(session)
        {
        }
    }
}