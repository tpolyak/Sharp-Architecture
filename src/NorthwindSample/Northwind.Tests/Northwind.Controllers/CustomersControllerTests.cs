using Northwind.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MvcContrib.TestHelper;
using SharpArch.Core.PersistenceSupport;
using Northwind.Core;
using Rhino.Mocks;
using NUnit.Framework.SyntaxHelpers;
using Northwind.Core.DataInterfaces;

namespace Tests.Northwind.Controllers
{
    [TestFixture]
    public class CustomersControllerTests
    {
        [SetUp]
        public void Setup() {
            testControllerBuilder = new TestControllerBuilder();
        }

        [Test]
        public void CanListCustomers() {
            CustomersController controller = 
                new CustomersController(CreateMockCustomerDao());
            testControllerBuilder.InitializeController(controller);

            ViewResult result = 
                controller.List().AssertViewRendered().ForView("");

            Assert.That(result.ViewData, Is.Not.Null);
            Assert.That(result.ViewData.Model as List<Customer>, Is.Not.Null);
            Assert.That((result.ViewData.Model as List<Customer>).Count, Is.EqualTo(2));
        }

        public ICustomerDao CreateMockCustomerDao() {
            MockRepository mocks = new MockRepository();

            ICustomerDao mockedCustomerDao = mocks.StrictMock<ICustomerDao>();
            Expect.Call(mockedCustomerDao.FindByCountry(null)).IgnoreArguments()
                .Return(CreateCustomers());
            mocks.Replay(mockedCustomerDao);

            return mockedCustomerDao;
        }

        private List<Customer> CreateCustomers() {
            List<Customer> customers = new List<Customer>();

            Customer customer = new Customer("Acme Anvil");
            PersistentObjectIdSetter<string>.SetIdOf(customer, "abcde");
            customers.Add(new Customer("Acme Anvil"));
            customers.Add(new Customer("Road Runner Industries"));

            return customers;
        }

        private TestControllerBuilder testControllerBuilder;
    }
}
