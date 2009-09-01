using Northwind.Data.NHibernateMaps;
using NUnit.Framework;
using SharpArch.Testing.NUnit.NHibernate;
using SharpArch.Data.NHibernate;
using System.Collections;
using NHibernate;
using System;
using System.Collections.Generic;
using NHibernate.Metadata;
using SharpArch.Testing.NHibernate;

namespace Tests.Northwind.Data.NHibernateMaps
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
            NHibernateSession.Init(
				new SimpleSessionStorage(), 
				mappingAssemblies, 
                new AutoPersistenceModelGenerator().Generate(),
                "../../../../app/Northwind.Web/NHibernate.config");
        }

        [Test]
        public void CanConfirmDatabaseMatchesMappings() {
			var allClassMetadata = NHibernateSession.GetDefaultSessionFactory().GetAllClassMetadata();

            foreach (var entry in allClassMetadata) {
                NHibernateSession.Current.CreateCriteria(entry.Value.GetMappedClass(EntityMode.Poco))
                     .SetMaxResults(0).List();
            }
        }

        [TearDown]
        public virtual void TearDown() {
			NHibernateSession.CloseAllSessions();
			NHibernateSession.Reset();
		}
    }
}
