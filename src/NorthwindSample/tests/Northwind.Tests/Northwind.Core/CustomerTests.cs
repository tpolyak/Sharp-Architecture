using NUnit.Framework;
using Northwind.Core;
using SharpArch.Core.DomainModel;
using NUnit.Framework.SyntaxHelpers;

namespace Tests.Northwind.Core
{
    [TestFixture]
    public class CustomerTests
    {
        [Test]
        public void CanCreateCustomer() {
            Customer customer = new Customer("Acme Anvil");
            Assert.That(customer.CompanyName, Is.EqualTo("Acme Anvil"));

            customer.CompanyName = "Acme 2";
            Assert.That(customer.CompanyName, Is.EqualTo("Acme 2"));

            Assert.That(customer.ContactName, Is.Null);
            customer.ContactName = "Billy";
            Assert.That(customer.ContactName, Is.EqualTo("Billy"));
        }

        [Test]
        public void CannotHaveValidCustomerWithoutCompanyName() {
            // Register the IValidator service
            ServiceLocatorSetup.InitServiceLocator();

            Customer customer = new Customer();
            Assert.That(customer.IsValid(), Is.False);

            customer.CompanyName = "Acme";
            Assert.That(customer.IsValid(), Is.True);
        }

        [Test]
        public void CanCompareCustomers() {
            Customer customerA = new Customer("Acme");
            Customer customerB = new Customer("Anvil");

            Assert.That(customerA, Is.Not.EqualTo(null));
            Assert.That(customerA, Is.Not.EqualTo(customerB));

            customerA.SetAssignedIdTo("AAAAA");
            customerB.SetAssignedIdTo("AAAAA");

            // Even though the "business value signatures" are different, the persistent IDs 
            // were the same.  Call me crazy, but I put that much trust into persisted IDs.
            Assert.That(customerA, Is.EqualTo(customerB));

            Customer customerC = new Customer("Acme");

            // Since customerA has an ID but customerC doesn't, they won't be equal
            // even though their signatures are the same
            Assert.That(customerA, Is.Not.EqualTo(customerC));

            Customer customerD = new Customer("Acme");

            // customerC and customerD are both transient and share the same signature
            Assert.That(customerC, Is.EqualTo(customerD));
        }
    }
}
