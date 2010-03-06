using NUnit.Framework;
using SharpArch.Data.NHibernate;
using Northwind.Core.Organization;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Testing.NUnit;
using SharpArch.Testing.NUnit.NHibernate;

namespace Tests.Northwind.Data.Organization
{
    [TestFixture]
    [Category("DB Tests")]
    public class EmployeeRepositoryTests : RepositoryTestsBase
    {
        protected override void LoadTestData() {
            Employee employee = new Employee("Joe", "Smith");
            employeeRepository.SaveOrUpdate(employee);
            FlushSessionAndEvict(employee);

            employee = new Employee("Andrew", "Fuller");
            employeeRepository.SaveOrUpdate(employee);
            FlushSessionAndEvict(employee);
        }

        /// <summary>
        /// WARNING: This is a very fragile test is will likely break over time.  It assumes 
        /// a particular employee exists in the database and has exactly 7 territories.  Fragile 
        /// tests that break over time can lead to people stopping to run tests at all.  In these
        /// instances, it's very important to use a test DB with known data.
        /// </summary>
        [Test]
        public void CanLoadEmployee() {
            Employee employeeFromDb = employeeRepository.Get(2);

            employeeFromDb.FirstName.ShouldEqual("Andrew");
            employeeFromDb.LastName.ShouldEqual("Fuller");
        }

        private IRepository<Employee> employeeRepository = new Repository<Employee>();
    }
}
