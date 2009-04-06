using NUnit.Framework;
using Northwind.Core.Organization;
using Northwind.Wcf.Dtos;
using NUnit.Framework.SyntaxHelpers;
using SharpArch.Testing;

namespace Tests.Northwind.Wcf.Dtos
{
    [TestFixture]
    public class EmployeeDtoTests
    {
        [Test]
        public void CanCreateDtoWithEntity() {
            Employee employee = new Employee("Steven", "Buchanan");
            EntityIdSetter.SetIdOf<int>(employee, 5);

            EmployeeDto employeeDto = EmployeeDto.Create(employee);

            Assert.That(employeeDto.Id, Is.EqualTo(5));
            Assert.That(employeeDto.FirstName, Is.EqualTo("Steven"));
            Assert.That(employeeDto.LastName, Is.EqualTo("Buchanan"));
        }
    }
}
