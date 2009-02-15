using NUnit.Framework;
using MvcContrib.TestHelper;
using Northwind.Web.Controllers.Organization;
using SharpArch.Core.PersistenceSupport;
using Northwind.Core.Organization;
using Rhino.Mocks;
using NUnit.Framework.SyntaxHelpers;
using System.Web.Mvc;
using System.Collections.Generic;
using SharpArch.Testing;
using SharpArch.Core.PersistenceSupport.NHibernate;
using SharpArch.Core.DomainModel;
using SharpArch.Core;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using CommonServiceLocator.WindsorAdapter;
using SharpArch.Core.CommonValidator;
using SharpArch.Core.NHibernateValidator.CommonValidatorAdapter;

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

            Assert.That(result.ViewData.Model as List<Employee>, Is.Not.Null);
            Assert.That((result.ViewData.Model as List<Employee>).Count, Is.EqualTo(2));
        }

        [Test]
        public void CanShowEmployee() {
            ViewResult result = controller.Show(1).AssertViewRendered();

            Assert.That(result.ViewData.Model as Employee, Is.Not.Null);
            Assert.That((result.ViewData.Model as Employee).Id, Is.EqualTo(1));
        }

        [Test]
        public void CanInitEmployeeCreation() {
            ViewResult result = controller.Create().AssertViewRendered();

            Assert.That(result.ViewData.Model as Employee, Is.Null);
        }

        [Test]
        public void CanCreateEmployee() {
            Employee employeeFromForm = new Employee() {
                FirstName = "Jackie",
                LastName = "Daniels",
                PhoneExtension = 350
            };

            RedirectToRouteResult redirectResult = controller.Create(employeeFromForm)
                .AssertActionRedirect().ToAction("Index");
            Assert.That(controller.TempData["message"].ToString().Contains("was successfully created"));
        }

        [Test]
        public void CanInitEmployeeEdit() {
            ViewResult result = controller.Edit(1).AssertViewRendered();

            Assert.That(result.ViewData.Model as Employee, Is.Not.Null);
            Assert.That((result.ViewData.Model as Employee).Id, Is.EqualTo(1));
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
            Assert.That(controller.TempData["message"], Is.EqualTo("Daniels, Jackie was successfully updated."));
        }

        [Test]
        public void CanDeleteEmployee() {
            RedirectToRouteResult redirectResult = controller.Delete(1)
                .AssertActionRedirect().ToAction("Index");
            Assert.That(controller.TempData["message"], Is.EqualTo("The employee was successfully deleted."));
        }

        private IRepository<Employee> CreateMockEmployeeRepository() {
            MockRepository mocks = new MockRepository();

            IRepository<Employee> mockedRepository = mocks.StrictMock<IRepository<Employee>>();
            Expect.Call(mockedRepository.GetAll())
                .Return(CreateEmployees());
            Expect.Call(mockedRepository.Get(1)).IgnoreArguments()
                .Return(CreateEmployee());
            Expect.Call(mockedRepository.SaveOrUpdate(null)).IgnoreArguments()
                .Return(CreateEmployee());
            Expect.Call(delegate { mockedRepository.Delete(null); }).IgnoreArguments();

            IDbContext mockedDbContext = mocks.StrictMock<IDbContext>();
            Expect.Call(delegate { mockedDbContext.CommitChanges(); });
            SetupResult.For(mockedRepository.DbContext).Return(mockedDbContext);

            mocks.Replay(mockedRepository);

            return mockedRepository;
        }

        private Employee CreateEmployee() {
            Employee employee = new Employee("Johnny", "Appleseed");
            EntityIdSetter<int>.SetIdOf(employee, 1);
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
