using SharpArch.Data.NHibernate;
using NUnit.Framework;
using SharpArch.Testing.NHibernate;

namespace SharpArch.Testing.NUnit.NHibernate
{
    /// <summary>
    /// Provides a base class for running unit tests against an in-memory database created
    /// during test execution.  This builds the database using the connection details within
    /// NHibernate.config.  If you'd prefer unit testing against a "live" development database
    /// such as a SQL Server instance, then use <see cref="DatabaseRepositoryTestsBase" /> instead.
    /// If you'd prefer a more behavior driven approach to testing against the in-memory database,
    /// use <see cref="RepositoryBehaviorSpecificationTestsBase" /> instead.
    /// </summary>
    public abstract class RepositoryTestsBase
    {
        [SetUp]
        protected virtual void SetUp() {
            RepositoryTestsHelper.InitializeDatabase();

            LoadTestData();
        }

        [TearDown]
        public virtual void TearDown() {
            NHibernateSession.Storage.Session.Dispose();
        }

        protected void FlushSessionAndEvict(object instance) {
            // Commits any changes up to this point to the database
            NHibernateSession.Storage.Session.Flush();

            // Evicts the instance from the current session so that it can be loaded during testing;
            // this gives the test a clean slate, if you will, to work with
            NHibernateSession.Storage.Session.Evict(instance);
        }

        protected abstract void LoadTestData();
    }
}