namespace SharpArch.Domain.PersistenceSupport.RavenDb
{
    using System;
    using System.Collections.Generic;

    public interface IRavenDbRepositoryWithTypeId<T, TIdT> : IRepositoryWithTypedId<T, TIdT>
    {
        #region Public Methods

        IEnumerable<T> FindAll(Func<T, bool> where);

        T FindOne(Func<T, bool> where);

        T First(Func<T, bool> where);

        #endregion
    }
}