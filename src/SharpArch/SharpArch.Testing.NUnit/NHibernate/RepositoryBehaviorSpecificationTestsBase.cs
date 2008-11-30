using FluentNHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using SharpArch.Core;
using System;
using SharpArch.Data.NHibernate;
using System.Configuration;
using System.Data;

namespace SharpArch.Testing.NUnit.NHibernate
{
    /// <summary>
    /// Provides a behavior driven approach to unit testsing data persisted to a database.  
    /// This base class creates the database from scratch using the specified connection details 
    /// within Hibernate.cfg.xml.  The original motivation for this class was to setup the database 
    /// within an in-memory SqlLite database, although there's nothing to prevent any other target 
    /// database being used.  If you'd prefer unit testing in the more traditional manner, but still 
    /// using an in-memory database, then use <see cref="RepositoryTestsBase" />.  For testing against
    /// a "live" development database, such as a SQL Server instance, use <see cref="DatabaseRepositoryTestsBase" />
    /// as your test fixture base class.
    /// </summary>
    public abstract class RepositoryBehaviorSpecificationTestsBase : BehaviorSpecificationTestsBase
    {
        [SetUp]
        protected override void SetUp() {
            RepositoryTestsHelper.InitializeDatabase();
            base.SetUp();
        }

        [TearDown]
        public virtual void TearDown() {
            NHibernateSession.Storage.Session.Dispose();
        }

        protected void FlushSessionAndEvict(object instance) {
            // Commits any changes up to this point to the database
            NHibernateSession.Storage.Session.Flush();

            // Evicts the instance from the current session so that it can be loaded during testing;
            // this gives the test a clean slate, if you will, to work with
            NHibernateSession.Storage.Session.Evict(instance);
        }
    }
}