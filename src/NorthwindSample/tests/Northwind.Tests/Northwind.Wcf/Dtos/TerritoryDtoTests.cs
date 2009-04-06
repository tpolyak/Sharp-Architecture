using NUnit.Framework;
using Northwind.Core;
using SharpArch.Testing;
using Northwind.Core.Organization;
using Northwind.Wcf.Dtos;
using NUnit.Framework.SyntaxHelpers;

namespace Tests.Northwind.Wcf.Dtos
{
    [TestFixture]
    public class TerritoryDtoTests
    {
        [Test]
        public void CanCreateDtoWithEntity() {
            Territory territory = CreateTerritory();

            TerritoryDto territoryDto = TerritoryDto.Create(territory);

            Assert.That(territoryDto.Id, Is.EqualTo("08837"));
            Assert.That(territoryDto.Description, Is.EqualTo("Edison"));

            Assert.That(territoryDto.RegionBelongingTo, Is.Not.Null);
            Assert.That(territoryDto.RegionBelongingTo.Id, Is.EqualTo(1));

            Assert.That(territoryDto.Employees.Count, Is.EqualTo(2));
            Assert.That(territoryDto.Employees[0].Id, Is.EqualTo(5));
            Assert.That(territoryDto.Employees[1].Id, Is.EqualTo(10));
        }

        private Territory CreateTerritory() {
            Territory territory = new Territory();
            EntityIdSetter.SetIdOf<string>(territory, "08837");
            territory.Description = "Edison";

            territory.RegionBelongingTo = new Region("Eastern");
            territory.RegionBelongingTo.SetAssignedIdTo(1);

            Employee employee1 = new Employee();
            EntityIdSetter.SetIdOf<int>(employee1, 5);
            territory.Employees.Add(employee1);

            Employee employee2 = new Employee();
            EntityIdSetter.SetIdOf<int>(employee2, 10);
            territory.Employees.Add(employee2);

            return territory;
        }
    }
}
