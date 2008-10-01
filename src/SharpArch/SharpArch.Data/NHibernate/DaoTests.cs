using NUnit.Framework;
using System.Configuration;

namespace SharpArch.Data.NHibernate
{
    /// <summary>
    /// Initiates a transaction before each test is run and rolls back the transaction after
    /// the test completes.  Consequently, tests make no permanent changes to the DB.
    /// </summary>
    public class DaoTests
    {
        [SetUp]
        public void SetUp() {
            NHibernateSession.Init(new SimpleSessionStorage(), 
                ConfigurationSettings.AppSettings["web.config"]);
            NHibernateSession.Current.BeginTransaction();
        }

        [TearDown]
        public void TearDown() {
            NHibernateSession.Current.Transaction.Rollback();
        }
    }
}
