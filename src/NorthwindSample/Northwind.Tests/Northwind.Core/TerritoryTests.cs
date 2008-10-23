using NUnit.Framework;
using Northwind.Core;
using NUnit.Framework.SyntaxHelpers;
using SharpArch.Core;

namespace Tests.Northwind.Core
{
    [TestFixture]
    public class TerritoryTests
    {
        [Test]
        public void CanCreateTerritory() {
            Region region = new RegionWithPublicConstructor("Midwest");

            Territory territory = new Territory("Cincinnati", region);
            Assert.That(territory.Description, Is.EqualTo("Cincinnati"));
            Assert.That(territory.RegionBelongingTo, Is.EqualTo(region));
            Assert.That(territory.ID, Is.Null);

            territory.SetAssignedIdTo("ABRACADABRA");
            Assert.That(territory.ID, Is.EqualTo("ABRACADABRA"));
        }

        [Test]
        [ExpectedException(typeof(PreconditionException))]
        public void CannotCreateTerritoryWithoutDescription() {
            new Territory(null, new RegionWithPublicConstructor("South"));
        }

        [Test]
        [ExpectedException(typeof(PreconditionException))]
        public void CannotCreateTerritoryWithoutRegion() {
            new Territory("Cincinnati", null);
        }
    }
}
