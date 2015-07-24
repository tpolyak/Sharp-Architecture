namespace SharpArch.Testing.NUnit.NHibernate
{
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
        private ISessionFactory sessionFactory;
        protected TransactionManager TransactionManager { get; private set; }

        protected ISession Session { get; private set; }


        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            sessionFactory = RepositoryTestsHelper.InitializeNHibernateSession().BuildSessionFactory();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            RepositoryTestsHelper.Shutdown(sessionFactory);
        }

        [TearDown]
        public virtual void TearDown()
        {
            RepositoryTestsHelper.Close(Session);
        }

        protected void FlushSessionAndEvict(object instance)
        {
            Session.FlushAndEvict(instance);
        }

        protected abstract void LoadTestData();

        [SetUp]
        protected virtual void SetUp()
        {
            Session = RepositoryTestsHelper.InitializeDatabase();
            TransactionManager = new TransactionManager(Session);
            this.LoadTestData();
        }
    }
}