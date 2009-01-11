using Northwind.Web.Controllers;
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
using SharpArch.Testing;

namespace Tests.Northwind.Web.Controllers
{
    [TestFixture]
    public class CustomersControllerTests
    {
        [Test]
        public void CanListCustomers() {
            CustomersController controller = 
                new CustomersController(CreateMockCustomerRepository());

            ViewResult result = 
                controller.Index().AssertViewRendered().ForView("");

            Assert.That(result.ViewData, Is.Not.Null);
            Assert.That(result.ViewData.Model as List<Customer>, Is.Not.Null);
            Assert.That((result.ViewData.Model as List<Customer>).Count, Is.EqualTo(2));
        }

        public ICustomerRepository CreateMockCustomerRepository() {
            MockRepository mocks = new MockRepository();

            ICustomerRepository mockedCustomerRepository = mocks.StrictMock<ICustomerRepository>();
            Expect.Call(mockedCustomerRepository.FindByCountry(null)).IgnoreArguments()
                .Return(CreateCustomers());
            mocks.Replay(mockedCustomerRepository);

            return mockedCustomerRepository;
        }

        private List<Customer> CreateCustomers() {
            List<Customer> customers = new List<Customer>();

            Customer customer = new Customer("Acme Anvil");
            PersistentObjectIdSetter<string>.SetIdOf(customer, "abcde");
            customers.Add(new Customer("Acme Anvil"));
            customers.Add(new Customer("Road Runner Industries"));

            return customers;
        }
    }
}
