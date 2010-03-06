using NUnit.Framework;
using Northwind.Core.Organization;
using Northwind.Wcf.Dtos;
using SharpArch.Testing;
using SharpArch.Testing.NUnit;

namespace Tests.Northwind.Wcf.Dtos
{
    [TestFixture]
    public class EmployeeDtoTests
    {
        [Test]
        public void CanCreateDtoWithEntity() {
            Employee employee = new Employee("Steven", "Buchanan");
            EntityIdSetter.SetIdOf(employee, 5);

            EmployeeDto employeeDto = EmployeeDto.Create(employee);

            employeeDto.Id.ShouldEqual(5);
            employeeDto.FirstName.ShouldEqual("Steven");
            employeeDto.LastName.ShouldEqual("Buchanan");
        }
    }
}
