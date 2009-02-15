using NUnit.Framework;
using Northwind.Core;
using NUnit.Framework.SyntaxHelpers;
using SharpArch.Core.DomainModel;

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
            Assert.That(territory.Id, Is.Null);

            territory.SetAssignedIdTo("ABRACADABRA");
            Assert.That(territory.Id, Is.EqualTo("ABRACADABRA"));
        }

        [Test]
        public void CannotHaveValidTerritoryWithoutDescriptionAndRegion() {
            // Register the IValidator service
            ServiceLocatorInitializer.Init();

            Territory territory = new Territory(null, null);
            Assert.That(territory.IsValid(), Is.False);
            Assert.That(territory.ValidationResults().Count, Is.EqualTo(2));

            territory.RegionBelongingTo = new RegionWithPublicConstructor("South");
            Assert.That(territory.IsValid(), Is.False);
            Assert.That(territory.ValidationResults().Count, Is.EqualTo(1));

            territory.Description = "Wherever";
            Assert.That(territory.IsValid(), Is.True);
        }
    }
}
