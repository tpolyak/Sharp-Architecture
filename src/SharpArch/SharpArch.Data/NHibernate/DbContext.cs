using SharpArch.Core.PersistenceSupport;
using NHibernate;

namespace SharpArch.Data.NHibernate
{
    /// <summary>
    /// This has to implement <see cref="IdbContext" />, but there's no reason for it to be 
    /// recreated more than once.  Therefore, it's been setup as a threadsafe singleton.
    /// Singleton guidance from http://www.yoda.arachsys.com/csharp/singleton.html.
    /// </summary>
    public sealed class DbContext : IDbContext
    {
        private DbContext() { }

        public static DbContext Instance {
            get {
                return Nested.instance;
            }
        }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }
            internal static readonly DbContext instance = new DbContext();
        }

        private ISession Session {
            get { return NHibernateSession.Current; }
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
    }
}
