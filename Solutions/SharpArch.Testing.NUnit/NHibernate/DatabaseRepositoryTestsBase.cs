namespace SharpArch.Testing.NUnit.NHibernate
{
    using global::NHibernate;
    using global::NHibernate.Cfg;
    using global::NUnit.Framework;

    /// <summary>
    ///     Initiates a transaction before each test is run and rolls back the transaction after
    ///     the test completes.  Consequently, tests make no permanent changes to the DB.
    ///     This is appropriate as a base class if you're unit tests run against a live, development
    ///     database.  If, alternatively, you'd prefer to use an in-memory database such as SqlLite,
    ///     then use <see cref="RepositoryTestsBase" /> or <see cref="RepositoryBehaviorSpecificationTestsBase" />
    ///     as your base class.
    ///     As the preferred mechanism is for in-memory unit testsing, this class is provided mainly
    ///     for backward compatibility.
    /// </summary>
    public abstract class DatabaseRepositoryTestsBase
    {
        protected ISession Session { get; private set; }
        private ISessionFactory sessionFactory;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var configuration = RepositoryTestsHelper.InitializeNHibernateSession();
            configuration = UpdateConfiguration(configuration);
            sessionFactory = configuration.BuildSessionFactory();
        }

        protected virtual Configuration UpdateConfiguration(Configuration configuration)
        {
            return configuration;
        }

        [SetUp]
        public virtual void SetUp()
        {
            Session = sessionFactory.OpenSession();
            Session.BeginTransaction();
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (Session != null)
            {
                Session.Transaction.Rollback();
                Session.Dispose();
                Session = null;
            }
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            if (sessionFactory != null)
            {
                sessionFactory.Dispose();
            }
        }
    }
}
