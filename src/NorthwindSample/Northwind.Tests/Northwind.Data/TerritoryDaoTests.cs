using NUnit.Framework;
using SharpArch.Data.NHibernate;
using Northwind.Core;
using SharpArch.Core.PersistenceSupport;
using NUnit.Framework.SyntaxHelpers;

namespace Tests.Northwind.Data
{
    [TestFixture]
    [Category("DB Tests")]
    public class TerritoryDaoTests : DaoTests
    {
        /// <summary>
        /// WARNING: This is a very fragile test is will likely break over time.  It assumes 
        /// a particular territory exists in the database and has exactly 1 employee.  Fragile 
        /// tests that break over time can lead to people stopping to run tests at all.  In these
        /// instances, it's very important to use a test DB with known data.
        /// </summary>
        [Test]
        public void CanLoadTerritory() {
            Territory territoryFromDb = territoryDao.Load("48084");

            Assert.That(territoryFromDb.Description.Trim(), Is.EqualTo("Troy"));
            Assert.That(territoryFromDb.RegionBelongingTo.Description.Trim(), Is.EqualTo("Northern"));
            Assert.That(territoryFromDb.Employees.Count, Is.EqualTo(1));
        }

        private IDaoWithTypedId<Territory, string> territoryDao = new GenericDaoWithTypedId<Territory, string>();
    }
}
