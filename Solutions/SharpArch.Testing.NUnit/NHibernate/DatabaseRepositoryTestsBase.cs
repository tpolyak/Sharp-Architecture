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
    /// <remarks>
    ///     This class expects database structure to be present and up-to-date. It will not run schema export on it.
    /// </remarks>
    public abstract class DatabaseRepositoryTestsBase
    {
        protected ISession Session { get; private set; }
        TestDatabaseInitializer initializer;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            initializer = new TestDatabaseInitializer(TestContext.CurrentContext.TestDirectory);
            UpdateConfiguration(initializer.GetConfiguration());
        }

        protected virtual void UpdateConfiguration(Configuration configuration)
        {
        }

        [SetUp]
        public virtual void SetUp()
        {
            Session = initializer.GetSessionFactory().OpenSession();
            Session.BeginTransaction();
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (Session != null)
            {
                if (Session.Transaction.IsActive)
                    Session.Transaction.Rollback();
                Session.Dispose();
                Session = null;
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            initializer?.Dispose();
            initializer = null;
        }
    }
}
