namespace SharpArch.NHibernate
{
    using System;

    using Domain;
    using Domain.PersistenceSupport;
    using global::NHibernate;

    public class DbContext : IDbContext
    {
        public DbContext(string factoryKey)
        {
            Check.Require(!string.IsNullOrEmpty(factoryKey), "factoryKey may not be null or empty");

            this.FactoryKey = factoryKey;
        }

        public string FactoryKey { get; set; }

        private ISession Session
        {
            get
            {
                return NHibernateSession.CurrentFor(this.FactoryKey);
            }
        }

        public IDisposable BeginTransaction()
        {
            return this.Session.BeginTransaction();
        }

        /// <summary>
        ///     This isn't specific to any one DAO and flushes everything that has been 
        ///     changed since the last commit.
        /// </summary>
        public void CommitChanges()
        {
            this.Session.Flush();
        }

        public void CommitTransaction()
        {
            this.Session.Transaction.Commit();
        }

        public void RollbackTransaction()
        {
            this.Session.Transaction.Rollback();
        }
    }
}