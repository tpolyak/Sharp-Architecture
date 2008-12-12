using NUnit.Framework;
using SharpArch.Testing.NUnit.NHibernate;
using SharpArch.Data.NHibernate;
using System.Collections;
using NHibernate;
using System;

namespace Tests.$solutionname$.Data.NHibernateMaps
{
    /// <summary>
    /// Provides a means to verify that the target database is in compliance with all mappings.
    /// Taken from http://ayende.com/Blog/archive/2006/08/09/NHibernateMappingCreatingSanityChecks.aspx.
    /// </summary>
    [TestFixture]
    [Category("DB Tests")]
    public class MappingIntegrationTests
    {
        [SetUp]
        public virtual void SetUp() {
            string[] mappingAssemblies = RepositoryTestsHelper.GetMappingAssemblies();
            NHibernateSession.Init(new SimpleSessionStorage(), mappingAssemblies, 
                "../../../../app/$solutionname$.Web/Hibernate.cfg.xml");
        }

        [Test]
        public void CanConfirmDatabaseMatchesMappings() {
            IDictionary allClassMetadata = NHibernateSession.SessionFactory.GetAllClassMetadata();
         
            foreach (DictionaryEntry entry in allClassMetadata) {
                NHibernateSession.Current.CreateCriteria((Type)entry.Key)
                     .SetMaxResults(0).List();
            }
        }

        [TearDown]
        public virtual void TearDown() {
            if (NHibernateSession.Storage.Session != null) {
                NHibernateSession.Storage.Session.Dispose();
            }
        }
    }
}
