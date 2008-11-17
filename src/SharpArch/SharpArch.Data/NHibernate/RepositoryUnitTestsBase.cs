using NUnit.Framework;
using System.Configuration;
using System.IO;
using NHibernate.Cfg;
using System.Collections.Specialized;
using SharpArch.Core;
using FluentNHibernate;
using System.Reflection;

namespace SharpArch.Data.NHibernate
{
    /// <summary>
    /// Initiates a transaction before each test is run and rolls back the transaction after
    /// the test completes.  Consequently, tests make no permanent changes to the DB.
    /// </summary>
    public abstract class RepositoryUnitTestsBase
    {
        [SetUp]
        public virtual void SetUp() {
            string nhibernateConfig = ConfigurationSettings.AppSettings["nhibernate.config.path"];
            string mappingAssemblies = ConfigurationSettings.AppSettings["nhibernate.mapping.assembly"];

            Check.Require(!string.IsNullOrEmpty(nhibernateConfig),
                "Please add an AppSetting to your app.config for 'nhibernate.config.path.' This setting " +
                "should be a relative path to the location of the configuration file containing NHibernate config options.");
            Check.Require(!string.IsNullOrEmpty(mappingAssemblies),
                "Please add an AppSetting to your app.config for 'nhibernate.mapping.assembly.' This setting " +
                "takes a comma delimited list of assemblies containing NHibernate mapping files. Including '.dll' " +
                "at the end of each is optional.");

            NHibernateSession.Init(new SimpleSessionStorage(), mappingAssemblies.Split(','), nhibernateConfig);
            NHibernateSession.Current.BeginTransaction();
        }

        [TearDown]
        public virtual void TearDown() {
            NHibernateSession.Current.Transaction.Rollback();
        }
    }
}
