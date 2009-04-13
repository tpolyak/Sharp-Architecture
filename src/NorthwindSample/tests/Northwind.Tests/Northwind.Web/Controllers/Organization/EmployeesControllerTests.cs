using NUnit.Framework;
using MvcContrib.TestHelper;
using Northwind.Web.Controllers.Organization;
using SharpArch.Core.PersistenceSupport;
using Northwind.Core.Organization;
using Rhino.Mocks;
using System.Web.Mvc;
using System.Collections.Generic;
using SharpArch.Testing;
using SharpArch.Core.DomainModel;
using SharpArch.Core;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using CommonServiceLocator.WindsorAdapter;
using SharpArch.Core.CommonValidator;
using SharpArch.Core.NHibernateValidator.CommonValidatorAdapter;
using SharpArch.Testing.NUnit;

namespace Tests.Northwind.Web.Controllers.Organization
{
    [TestFixture]
    public class EmployeesControllerTests
    {
        [SetUp]
        public void SetUp() {
            InitServiceLocator();

            controller = new EmployeesController(CreateMockEmployeeRepository());
        }

        public void InitServiceLocator() {
            IWindsorContainer container = new WindsorContainer();
            container.AddComponent("duplicateChecker",
                typeof(IEntityDuplicateChecker), typeof(DuplicateCheckerStub));
            container.AddComponent("validator", typeof(IValidator), typeof(Validator));
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }

        [Test]
        public void CanListEmployees() {
            ViewResult result = controller.Index().AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            (result.ViewData.Model as List<Employee>).Count.ShouldEqual(2);
        }

        [Test]
        public void CanShowEmployee() {
            ViewResult result = controller.Show(1).AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            (result.ViewData.Model as Employee).Id.ShouldEqual(1);
        }

        [Test]
        public void CanInitEmployeeCreation() {
            ViewResult result = controller.Create().AssertViewRendered();

            result.ViewData.Model.ShouldBeNull();
        }

        [Test]
        public void CanCreateEmployee() {
            Employee employeeFromForm = new Employee
                                        {
                FirstName = "Jackie",
                LastName = "Daniels",
                PhoneExtension = 350
            };

            RedirectToRouteResult redirectResult = controller.Create(employeeFromForm)
                .AssertActionRedirect().ToAction("Index");
            controller.TempData["message"].ToString().ShouldContain("was successfully created");
        }

        [Test]
        public void CanInitEmployeeEdit() {
            ViewResult result = controller.Edit(1).AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            (result.ViewData.Model as Employee).Id.ShouldEqual(1);
        }

        [Test]
        public void CanUpdateEmployee() {
            Employee employeeFromForm = new Employee() {
                FirstName = "Jackie",
                LastName = "Daniels",
                PhoneExtension = 350
            };
            RedirectToRouteResult redirectResult = controller.Edit(1, employeeFromForm)
                .AssertActionRedirect().ToAction("Index");
            controller.TempData["message"].ShouldEqual("Daniels, Jackie was successfully updated.");
        }

        [Test]
        public void CanDeleteEmployee() {
            RedirectToRouteResult redirectResult = controller.Delete(1)
                .AssertActionRedirect().ToAction("Index");
            controller.TempData["message"].ShouldEqual("The employee was successfully deleted.");
        }

        private IRepository<Employee> CreateMockEmployeeRepository() {
            IRepository<Employee> repository = MockRepository.GenerateMock<IRepository<Employee>>( );
            repository.Expect(r => r.GetAll()).Return(CreateEmployees());
            repository.Expect(r => r.Get(1)).IgnoreArguments().Return(CreateEmployee());
            repository.Expect(r => r.SaveOrUpdate(null)).IgnoreArguments().Return(CreateEmployee());
            repository.Expect(r => r.Delete(null)).IgnoreArguments();


            IDbContext dbContext = MockRepository.GenerateStub<IDbContext>();
            dbContext.Stub(c => c.CommitChanges());
            repository.Stub(r => r.DbContext).Return(dbContext);

            return repository;
        }

        private Employee CreateEmployee() {
            Employee employee = new Employee("Johnny", "Appleseed");
            EntityIdSetter.SetIdOf(employee, 1);
            return employee;
        }

        private List<Employee> CreateEmployees() {
            List<Employee> employees = new List<Employee>();

            employees.Add(new Employee("John", "Wayne"));
            employees.Add(new Employee("Joe", "Bradshaw"));

            return employees;
        }

        private EmployeesController controller;

        private class DuplicateCheckerStub : IEntityDuplicateChecker
        {
            public bool DoesDuplicateExistWithTypedIdOf<IdT>(IEntityWithTypedId<IdT> entity) {
                Check.Require(entity != null);

                if (entity as Employee != null) {
                    Employee employee = entity as Employee;
                    return employee.FirstName == "Billy" && employee.FirstName == "McCafferty";
                }

                // By default, simply return false for no duplicates found
                return false;
            }
        }
    }
}
