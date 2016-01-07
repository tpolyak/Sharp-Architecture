namespace SharpArch.Testing.NUnit.NHibernate
{
    using System.Diagnostics;
    using global::NHibernate;
    using global::NUnit.Framework;
    using SharpArch.NHibernate;

    /// <summary>
    ///     Provides a base class for running unit tests against an in-memory database created
    ///     during test execution.  This builds the database using the connection details within
    ///     NHibernate.config.  If you'd prefer unit testing against a "live" development database
    ///     such as a SQL Server instance, then use <see cref = "DatabaseRepositoryTestsBase" /> instead.
    ///     If you'd prefer a more behavior driven approach to testing against the in-memory database,
    ///     use <see cref = "RepositoryBehaviorSpecificationTestsBase" /> instead.
    /// </summary>
    public abstract class RepositoryTestsBase
    {
        protected TransactionManager TransactionManager { get; private set; }

        protected ISession Session { get; private set; }

        TestDatabaseInitializer dbInitializer;


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            dbInitializer = new TestDatabaseInitializer(TestContext.CurrentContext.TestDirectory);
            dbInitializer.GetSessionFactory();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            dbInitializer?.Dispose();
            dbInitializer = null;
        }

        [TearDown]
        public virtual void TearDown()
        {
            TestDatabaseInitializer.Close(Session);
        }

        protected void FlushSessionAndEvict(object instance)
        {
            Session.FlushAndEvict(instance);
        }

        protected abstract void LoadTestData();

        [SetUp]
        protected virtual void SetUp()
        {
            Session = dbInitializer.InitializeSession();
            TransactionManager = new TransactionManager(Session);
            this.LoadTestData();
        }
    }
}