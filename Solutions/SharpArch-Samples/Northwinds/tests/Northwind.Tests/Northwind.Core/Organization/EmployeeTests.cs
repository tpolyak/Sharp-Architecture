using NUnit.Framework;
using Northwind.Core.Organization;
using Northwind.Core;
using SharpArch.Testing.NUnit;

namespace Tests.Northwind.Core.Organization
{
    [TestFixture]
    public class EmployeeTests
    {
        [Test]
        public void CanCreateEmployee() {
            Employee employee = new Employee("Aunt", "Jamima");

            employee.FirstName.ShouldEqual("Aunt");
            employee.LastName.ShouldEqual("Jamima");
            employee.Territories.Count.ShouldEqual(0);
        }

        [Test]
        public void CanAssociateTerritoriesWithEmployee() {
            Employee employee = new Employee("Mrs.", "Butterworths");
            employee.Territories.Add(new Territory("Cincinnati", new RegionWithPublicConstructor("Midwest")));
            employee.Territories.Add(new Territory("Seattle", new RegionWithPublicConstructor("Northwest")));

            employee.Territories.Count.ShouldEqual(2);
        }
    }
}
