namespace Suteki.TardisBank.Tests.Suteki.TardisBank.Data.NHibernateMaps
{
    using System.IO;

    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;

    using NUnit.Framework;

    using SharpArch.NHibernate;
    using SharpArch.Testing.NUnit.NHibernate;

    using global::Suteki.TardisBank.Infrastructure.NHibernateMaps;

    /// <summary>
    /// Provides a means to verify that the target database is in compliance with all mappings.
    /// Taken from http://ayende.com/Blog/archive/2006/08/09/NHibernateMappingCreatingSanityChecks.aspx.
    /// 
    /// If this is failing, the error will likely inform you that there is a missing table or column
    /// which needs to be added to your database.
    /// </summary>
    [TestFixture]
    [Category("DB Tests")]
    public class MappingIntegrationTests
    {
        private Configuration configuration;

        [SetUp]
        public virtual void SetUp()
        {
            string[] mappingAssemblies = RepositoryTestsHelper.GetMappingAssemblies();
            this.configuration = NHibernateSession.Init(
                new SimpleSessionStorage(),
                mappingAssemblies,
                new AutoPersistenceModelGenerator().Generate(),
                "../../../../Solutions/Suteki.TardisBank.Web.Mvc/NHibernate.config");
        }

        [TearDown]
        public virtual void TearDown()
        {
            NHibernateSession.CloseAllSessions();
            NHibernateSession.Reset();
        }

        [Test]
        public void CanConfirmDatabaseMatchesMappings()
        {
            var allClassMetadata = NHibernateSession.GetDefaultSessionFactory().GetAllClassMetadata();

            foreach (var entry in allClassMetadata)
            {
                NHibernateSession.Current.CreateCriteria(entry.Value.GetMappedClass(EntityMode.Poco))
                     .SetMaxResults(0).List();
            }
        }

        /// <summary>
        /// Generates and outputs the database schema SQL to the console
        /// </summary>
        [Test]
        public void CanGenerateDatabaseSchema()
        {
            var session = NHibernateSession.GetDefaultSessionFactory().OpenSession();

            using (TextWriter stringWriter = new StreamWriter("../../../../Database/UnitTestGeneratedSchema.sql"))
            {
                new SchemaExport(this.configuration).Execute(true, false, false, session.Connection, stringWriter);
            }
        }

        /// <summary>
        /// Creates/Updates database schema, this runs on database configured in 
        /// Mvc project and is marked as Explicit because it changes the database.
        /// </summary>
        [Test]
        [Explicit]
        public void CanCreateDatabase()
        {
            new SchemaExport(this.configuration).Execute(false, true, false);
        }
    }
}
