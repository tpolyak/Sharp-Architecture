using NUnit.Framework;
using System.Configuration;
using System.IO;
using NHibernate.Cfg;
using System.Collections.Specialized;

namespace SharpArch.Data.NHibernate
{
    /// <summary>
    /// Initiates a transaction before each test is run and rolls back the transaction after
    /// the test completes.  Consequently, tests make no permanent changes to the DB.
    /// </summary>
    public abstract class RepositoryUnitTestsBase
    {
        [SetUp]
        public void SetUp() {
            string webNHibernateConfig = ConfigurationSettings.AppSettings["web.config"];

            if (! string.IsNullOrEmpty(webNHibernateConfig)) {
                NHibernateSession.Init(new SimpleSessionStorage(), webNHibernateConfig);
            }
            else {
                NHibernateSession.Init(new SimpleSessionStorage());
            }

            NHibernateSession.Current.BeginTransaction();
        }

        [TearDown]
        public void TearDown() {
            NHibernateSession.Current.Transaction.Rollback();
        }
    }
}
