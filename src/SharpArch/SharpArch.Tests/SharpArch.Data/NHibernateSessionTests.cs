using FluentNHibernate.Cfg.Db;
using NUnit.Framework;
using SharpArch.Data.NHibernate;

namespace Tests.SharpArch.Data
{
    [TestFixture]
    public class NHibernateSessionTests
    {
        [SetUp]
        public void SetUp() {
            NHibernateSession.RemoveSessionFactoryFor(NHibernateSession.DefaultFactoryKey);
            NHibernateSession.Storage = null;
        }

        [Test]
        public void CanInitializeWithConfigFile() {
            var configFile = "sqlite-nhibernate-config.xml";
            var mappingAssemblies = new string[] { };
            var configuration = NHibernateSession.Init(
                new SimpleSessionStorage(),
                mappingAssemblies, configFile);

            Assert.That(configuration, Is.Not.Null);
        }

        [Test]
        public void CanInitializeWithPersistenceConfigurerAndConfigFile() {
            var configFile = "sqlite-nhibernate-config.xml";
            var persistenceConfigurer = SQLiteConfiguration.Standard
                .ConnectionString(c => c.Is("Data Source=:memory:;Version=3;New=True;"))
                .ProxyFactoryFactory("NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");

            var mappingAssemblies = new string[] { };

            var configuration = NHibernateSession.Init(
                new SimpleSessionStorage(),
                mappingAssemblies, null, configFile, null, null, persistenceConfigurer);

            Assert.That(configuration, Is.Not.Null);
        }

        [Test]
        public void CanInitializeWithPersistenceConfigurerAndNoConfigFile() {
            var persistenceConfigurer = SQLiteConfiguration.Standard
                .ConnectionString(c => c.Is("Data Source=:memory:;Version=3;New=True;"))
                .ProxyFactoryFactory("NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");

            var mappingAssemblies = new string[] { };

            var configuration = NHibernateSession.Init(
                new SimpleSessionStorage(),
                mappingAssemblies, null, null, null, null, persistenceConfigurer);

            Assert.That(configuration, Is.Not.Null);

        }
    }
}