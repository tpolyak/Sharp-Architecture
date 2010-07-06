using NUnit.Framework;
using SharpArch.Data.NHibernate;
using Northwind.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Testing.NUnit;
using SharpArch.Testing.NUnit.NHibernate;
using SharpArch.Core.PersistenceSupport.NHibernate;
using Northwind.Core.Organization;

namespace Tests.Northwind.Data
{
    [TestFixture]
    [Category("DB Tests")]
    public class TerritoryRepositoryTests : RepositoryTestsBase
    {
        protected override void LoadTestData() {
            Region region = new Region("Northern");
            region.SetAssignedIdTo(1);
            regionRepository.Save(region);
            FlushSessionAndEvict(region);

            Territory territory = new Territory("Troy", region);
            territory.SetAssignedIdTo("48084");
            territoryRepository.Save(territory);
            FlushSessionAndEvict(territory);

            Employee employee = new Employee("Joe", "Smith");
            employee.Territories.Add(territory);
            employeeRepository.SaveOrUpdate(employee);
            FlushSessionAndEvict(employee);
        }

        /// <summary>
        /// WARNING: This is a very fragile test is will likely break over time.  It assumes 
        /// a particular territory exists in the database and has exactly 1 employee.  Fragile 
        /// tests that break over time can lead to people stopping to run tests at all.  In these
        /// instances, it's very important to use a test DB with known data.
        /// </summary>
        [Test]
        public void CanLoadTerritory() {
            Territory territoryFromDb = territoryRepository.Get("48084");

            territoryFromDb.Description.Trim().ShouldEqual("Troy");
            territoryFromDb.RegionBelongingTo.Description.Trim().ShouldEqual("Northern");
            territoryFromDb.Employees.Count.ShouldEqual(1);
        }

        private IRepository<Employee> employeeRepository =
            new Repository<Employee>();
        private INHibernateRepositoryWithTypedId<Territory, string> territoryRepository = 
            new NHibernateRepositoryWithTypedId<Territory, string>();
        private INHibernateRepository<Region> regionRepository =
            new NHibernateRepository<Region>();
    }
}
