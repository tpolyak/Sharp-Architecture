using NUnit.Framework;
using Northwind.Core;
using SharpArch.Core.DomainModel;
using SharpArch.Testing.NUnit;

namespace Tests.Northwind.Core
{
    [TestFixture]
    public class TerritoryTests
    {
        [Test]
        public void CanCreateTerritory() {
            Region region = new RegionWithPublicConstructor("Midwest");

            Territory territory = new Territory("Cincinnati", region);
            territory.Description.ShouldEqual("Cincinnati");
            territory.RegionBelongingTo.ShouldEqual(region);
            territory.Id.ShouldBeNull();

            territory.SetAssignedIdTo("ABRACADABRA");
            territory.Id.ShouldEqual("ABRACADABRA");
        }

        [Test]
        public void CannotHaveValidTerritoryWithoutDescriptionAndRegion() {
            // Register the IValidator service
            ServiceLocatorInitializer.Init();

            Territory territory = new Territory(null, null);
            territory.IsValid().ShouldBeFalse();
            territory.ValidationResults().Count.ShouldEqual(2);

            territory.RegionBelongingTo = new RegionWithPublicConstructor("South");
            territory.IsValid().ShouldBeFalse();
            territory.ValidationResults().Count.ShouldEqual(1);

            territory.Description = "Wherever";
            territory.IsValid().ShouldBeTrue();
        }
    }
}
