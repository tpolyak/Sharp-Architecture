using System.Collections.Generic;
using NHibernate;
using NHibernate.Metadata;
using NUnit.Framework;
using $solutionname$.Data.NHibernateMaps;
using SharpArch.Data.NHibernate;
using SharpArch.Testing.NHibernate;

namespace Tests.$solutionname$.Data.NHibernateMaps
{
    /// <summary>
    /// Provides a means to verify that the target database is in compliance with all mappings.
    /// Taken from http://ayende.com/Blog/archive/2006/08/09/NHibernateMappingCreatingSanityChecks.aspx.
    /// 
    /// If this is failing, the error will likely inform you that there is a missing table or column
    /// which needs to be added to your database.
    /// </summary>
    [TestFixture]
    [Category("DB Tests")]
    public class MappingIntegrationTests {
        #region Setup/Teardown

        [SetUp]
        public virtual void SetUp() {

            string[] mappingAssemblies = RepositoryTestsHelper.GetMappingAssemblies();
            NHibernateSession.Init(new SimpleSessionStorage(), mappingAssemblies,
                                   new AutoPersistenceModelGenerator().Generate(),
                                   "../../../../app/$solutionname$.Web/NHibernate.config");
        }

        [TearDown]
        public virtual void TearDown()
        {
            NHibernateSession.CloseAllSessions();
            NHibernateSession.Reset();
        }

        #endregion

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
    }
}
