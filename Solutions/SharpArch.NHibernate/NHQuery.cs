namespace SharpArch.NHibernate
{
    using System.Collections.Generic;

    using global::NHibernate;

    using SharpArch.Domain.PersistenceSupport;

    public abstract class NHQuery<T> : IQuery<T>
    {
        protected virtual ISession Session
        {
            get
            {
                string factoryKey = SessionFactoryKeyHelper.GetKeyFrom(this);
                return NHibernateSession.CurrentFor(factoryKey);
            }
        }

        #region Implementation of IQuery<T>

        public abstract IList<T> ExecuteQuery();

        #endregion
    }
}