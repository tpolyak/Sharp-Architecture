using SharpArch.Core.PersistenceSupport;
using NHibernate;
using SharpArch.Core;

namespace SharpArch.Data.NHibernate
{
    public class DbContext : IDbContext
    {
        public DbContext(string factoryKey) {
            Check.Require(!string.IsNullOrEmpty(factoryKey), "factoryKey may not be null or empty");

            FactoryKey = factoryKey;
        }

        private ISession Session {
            get {
                return NHibernateSession.CurrentFor(FactoryKey);
            }
                
        }

        /// <summary>
        /// This isn't specific to any one DAO and flushes everything that has been 
        /// changed since the last commit.
        /// </summary>
        public void CommitChanges() {
            Session.Flush();
        }

        public void BeginTransaction() {
            Session.BeginTransaction();
        }

        public void CommitTransaction() {
            Session.Transaction.Commit();
        }

        public void RollbackTransaction() {
            Session.Transaction.Rollback();
        }

        public string FactoryKey { get; set; }
    }
}
