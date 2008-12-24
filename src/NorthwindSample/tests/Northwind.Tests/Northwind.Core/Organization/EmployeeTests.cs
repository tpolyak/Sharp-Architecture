using NUnit.Framework;
using Northwind.Core.Organization;
using NUnit.Framework.SyntaxHelpers;
using Northwind.Core;

namespace Tests.Northwind.Core.Organization
{
    [TestFixture]
    public class EmployeeTests
    {
        [Test]
        public void CanCreateEmployee() {
            Employee employee = new Employee("Aunt", "Jamima");

            Assert.That(employee.FirstName, Is.EqualTo("Aunt"));
            Assert.That(employee.LastName, Is.EqualTo("Jamima"));
            Assert.That(employee.Territories.Count, Is.EqualTo(0));
        }

        [Test]
        public void CanAssociateTerritoriesWithEmployee() {
            Employee employee = new Employee("Mrs.", "Butterworths");
            employee.Territories.Add(new Territory("Cincinnati", new RegionWithPublicConstructor("Midwest")));
            employee.Territories.Add(new Territory("Seattle", new RegionWithPublicConstructor("Northwest")));

            Assert.That(employee.Territories.Count, Is.EqualTo(2));
        }
    }
}
