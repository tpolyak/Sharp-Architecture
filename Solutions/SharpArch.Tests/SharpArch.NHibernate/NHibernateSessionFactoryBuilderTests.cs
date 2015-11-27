using FluentAssertions;
using SharpArch.Testing.NUnit;

namespace Tests.SharpArch.NHibernate
{
    using System;
    using System.IO;
    using FluentNHibernate.Cfg.Db;
    using global::SharpArch.NHibernate;
    using NUnit.Framework;

    [TestFixture]
    public class NHibernateSessionFactoryBuilderTests
    {
        [Test]
        public void CanInitializeWithConfigFile()
        {
            var configFile = "sqlite-nhibernate-config.xml";

            var configuration = new NHibernateSessionFactoryBuilder()
                .UseConfigFile(configFile)
                .BuildConfiguration();

            Assert.That(configuration, Is.Not.Null);

            configuration.BuildSessionFactory();
        }

        [Test]
        public void CanInitializeWithConfigFileAndConfigurationFileCache()
        {
            var configFile = "sqlite-nhibernate-config.xml";

            var configuration = new NHibernateSessionFactoryBuilder()
                .UseConfigurationCache(new NHibernateConfigurationFileCache(new[] {"SharpArch.NHibernate"}))
                .UseConfigFile(configFile)
                .BuildConfiguration();

            Assert.That(configuration, Is.Not.Null);

            configuration.BuildSessionFactory();
        }

        [Test]
        public void CanInitializeWithPersistenceConfigurerAndConfigFile()
        {
            var configFile = "sqlite-nhibernate-config.xml";
            var persistenceConfigurer =
                SQLiteConfiguration.Standard.ConnectionString(c => c.Is("Data Source=:memory:;Version=3;New=True;"));

            var configuration = new NHibernateSessionFactoryBuilder()
                .UsePersistenceConfigurer(persistenceConfigurer)
                .UseConfigFile(configFile)
                .BuildConfiguration();

            Assert.That(configuration, Is.Not.Null);
            configuration.BuildSessionFactory();
        }

        [Test]
        public void CanInitializeWithPersistenceConfigurerAndNoConfigFile()
        {
            var persistenceConfigurer =
                SQLiteConfiguration.Standard.ConnectionString(c => c.Is("Data Source=:memory:;Version=3;New=True;"));

            var configuration = new NHibernateSessionFactoryBuilder()
                .UsePersistenceConfigurer(persistenceConfigurer)
                .BuildConfiguration();

            Assert.That(configuration, Is.Not.Null);
            configuration.BuildSessionFactory();
        }

        [Test]
        public void DoesInitializeFailWhenCachingFileDependencyCannotBeFound()
        {
            Assert.Throws<FileNotFoundException>(
                () =>
                {
                    var configFile = "sqlite-nhibernate-config.xml";
                    var mappingAssemblies = new string[] {};

                    var configuration = new NHibernateSessionFactoryBuilder()
                        // Random Guid value as dependency file to cause the exception
                        .UseConfigurationCache(new NHibernateConfigurationFileCache(new[] {Guid.NewGuid().ToString()}))
                        .UseConfigFile(configFile)
                        .BuildConfiguration();
                });
        }

        [Test]
        public void WhenUsingDataAnnotationValidators_ShouldKeepRegisteredPreInsertEventListeners()
        {

            var configFile = "sqlite-nhibernate-config.xml";

            var configuration = new NHibernateSessionFactoryBuilder()
                .UseConfigFile(configFile)
                .UseDataAnnotationValidators(true)
                .BuildConfiguration();

            configuration.EventListeners.PreInsertEventListeners.Should().Contain(l => l is PreInsertListener);
        }

        [Test]
        public void WhenUsingDataAnnotationValidators_ShouldKeepRegisteredPreUpdateEventListeners()
        {

            var configFile = "sqlite-nhibernate-config.xml";

            var configuration = new NHibernateSessionFactoryBuilder()
                .UseConfigFile(configFile)
                .UseDataAnnotationValidators(true)
                .BuildConfiguration();

            configuration.EventListeners.PreUpdateEventListeners.Should().Contain(l => l is PreUpdateListener);
        }

    }
}
