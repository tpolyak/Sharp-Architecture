using Northwind.Core.Organization;
using SharpArch.Core.PersistenceSupport;
using System.Collections.Generic;
using SharpArch.Testing;
using Rhino.Mocks;

namespace Tests.Northwind.Data.TestDoubles
{
    public class MockEmployeeRepositoryFactory
    {
        public static IRepository<Employee> CreateMockEmployeeRepository() {
            IRepository<Employee> mockedRepository = MockRepository.GenerateMock<IRepository<Employee>>();
            mockedRepository.Expect(mr => mr.GetAll()).Return(CreateEmployees());
            mockedRepository.Expect(mr => mr.Get(1)).IgnoreArguments().Return(CreateEmployee());
            mockedRepository.Expect(mr => mr.SaveOrUpdate(null)).IgnoreArguments().Return(CreateEmployee());
            mockedRepository.Expect(mr => mr.Delete(null)).IgnoreArguments();

            IDbContext mockedDbContext = MockRepository.GenerateStub<IDbContext>();
            mockedDbContext.Stub(c => c.CommitChanges());
            mockedRepository.Stub(mr => mr.DbContext).Return(mockedDbContext);

            return mockedRepository;
        }

        /// <summary>
        /// Creates a valid, transient Employee; typical of something retrieved back from a form submission
        /// </summary>
        public static Employee CreateTransientEmployee() {
            Employee employee = new Employee() {
                FirstName = "Jackie",
                LastName = "Daniels",
                PhoneExtension = 5491
            };

            return employee;
        }

        private static Employee CreateEmployee() {
            Employee employee = CreateTransientEmployee();
            EntityIdSetter.SetIdOf<int>(employee, 1);
            return employee;
        }

        private static List<Employee> CreateEmployees() {
            List<Employee> employees = new List<Employee>();

            // Create a number of domain object instances here and add them to the list

            return employees;
        }
    }
}
