using NUnit.Framework;
using Northwind.Core;
using SharpArch.Testing;
using Northwind.Wcf.Dtos;
using NUnit.Framework.SyntaxHelpers;

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

            Assert.That(regionDto.Id, Is.EqualTo(1));
            Assert.That(regionDto.Description, Is.EqualTo("Eastern"));
        }
    }
}
