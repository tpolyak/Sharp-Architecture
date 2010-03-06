using NUnit.Framework;
using Northwind.Core;
using Northwind.Wcf.Dtos;
using SharpArch.Testing.NUnit;

namespace Tests.Northwind.Wcf.Dtos
{
    [TestFixture]
    public class RegionDtoTests
    {
        [Test]
        public void CanCreateDtoWithEntity() {
            Region region = new Region("Eastern");
            region.SetAssignedIdTo(1);

            RegionDto regionDto = RegionDto.Create(region);

            regionDto.Id.ShouldEqual(1);
            regionDto.Description.ShouldEqual("Eastern");
        }
    }
}
