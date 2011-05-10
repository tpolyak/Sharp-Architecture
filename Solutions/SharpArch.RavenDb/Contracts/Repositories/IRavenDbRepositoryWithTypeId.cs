namespace SharpArch.RavenDb.Contracts.Repositories
{
    using System;
    using System.Collections.Generic;

    using SharpArch.Domain.PersistenceSupport;

    public interface IRavenDbRepositoryWithTypeId<T, TIdT> : IRepositoryWithTypedId<T, TIdT>
    {
        #region Public Methods

        IEnumerable<T> FindAll(Func<T, bool> where);

        T FindOne(Func<T, bool> where);

        T First(Func<T, bool> where);

        #endregion
    }
}