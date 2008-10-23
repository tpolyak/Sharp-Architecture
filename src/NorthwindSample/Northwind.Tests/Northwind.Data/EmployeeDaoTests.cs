using NUnit.Framework;
using SharpArch.Data.NHibernate;
using Northwind.Core;
using SharpArch.Core.PersistenceSupport;
using NUnit.Framework.SyntaxHelpers;

namespace Tests.Northwind.Data
{
    [TestFixture]
    [Category("DB Tests")]
    public class EmployeeDaoTests : DaoTests
    {
        /// <summary>
        /// WARNING: This is a very fragile test is will likely break over time.  It assumes 
        /// a particular employee exists in the database and has exactly 7 territories.  Fragile 
        /// tests that break over time can lead to people stopping to run tests at all.  In these
        /// instances, it's very important to use a test DB with known data.
        /// </summary>
        [Test]
        public void CanLoadEmployee() {
            Employee employeeFromDb = employeeDao.Load(2);

            Assert.That(employeeFromDb.FirstName, Is.EqualTo("Andrew"));
            Assert.That(employeeFromDb.LastName, Is.EqualTo("Fuller"));
            Assert.That(employeeFromDb.Territories.Count, Is.EqualTo(7));
        }

        private IDao<Employee> employeeDao = new GenericDao<Employee>();
    }
}
