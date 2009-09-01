using SharpArch.Data.NHibernate;
using NUnit.Framework;
using SharpArch.Testing.NHibernate;

namespace SharpArch.Testing.NUnit.NHibernate
{
	/// <summary>
	/// Provides a behavior driven approach to unit testsing data persisted to a database.  
	/// This base class creates the database from scratch using the specified connection details 
	/// within NHibernate.config.  The original motivation for this class was to setup the database 
	/// within an in-memory SqlLite database, although there's nothing to prevent any other target 
	/// database being used.  If you'd prefer unit testing in the more traditional manner, but still 
	/// using an in-memory database, then use <see cref="RepositoryTestsBase" />.  For testing against
	/// a "live" development database, such as a SQL Server instance, use <see cref="DatabaseRepositoryTestsBase" />
	/// as your test fixture base class.
	/// </summary>
	public abstract class RepositoryBehaviorSpecificationTestsBase : BehaviorSpecificationTestsBase
	{
		[SetUp]
		protected override void SetUp()
		{
			RepositoryTestsHelper.InitializeDatabase();
			base.SetUp();
		}

		[TearDown]
		public virtual void TearDown()
		{
			RepositoryTestsHelper.Shutdown();
		}

		protected void FlushSessionAndEvict(object instance)
		{
			RepositoryTestsHelper.FlushSessionAndEvict(instance);
		}
	}
}