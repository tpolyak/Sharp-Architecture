using NUnit.Framework;
using Northwind.Core;
using SharpArch.Testing.NUnit;

namespace Tests.Northwind.Core
{
    [TestFixture]
    public class CustomerTests
    {
        [Test]
        public void CanCreateCustomer() {
            Customer customer = new Customer("Acme Anvil");
            customer.CompanyName.ShouldEqual("Acme Anvil");

            customer.CompanyName = "Acme 2";
            customer.CompanyName.ShouldEqual("Acme 2");

            customer.ContactName.ShouldBeNull();
            customer.ContactName = "Billy";
            customer.ContactName.ShouldEqual("Billy");
        }

        [Test]
        public void CannotHaveValidCustomerWithoutCompanyName() {
            // Register the IValidator service
            ServiceLocatorInitializer.Init();

            Customer customer = new Customer();
            customer.IsValid().ShouldBeFalse();

            customer.CompanyName = "Acme";
            customer.IsValid().ShouldBeTrue();
        }

        [Test]
        public void CanCompareCustomers() {
            Customer customerA = new Customer("Acme");
            Customer customerB = new Customer("Anvil");

            customerA.ShouldNotEqual(null);
            customerA.ShouldNotEqual(customerB);

            customerA.SetAssignedIdTo("AAAAA");
            customerB.SetAssignedIdTo("AAAAA");

            // Even though the "business value signatures" are different, the persistent IDs 
            // were the same.  Call me crazy, but I put that much trust into persisted IDs.
            customerA.ShouldEqual(customerB);

            Customer customerC = new Customer("Acme");

            // Since customerA has an ID but customerC doesn't, they won't be equal
            // even though their signatures are the same
            customerA.ShouldNotEqual(customerC);

            Customer customerD = new Customer("Acme");

            // customerC and customerD are both transient and share the same signature
            customerC.ShouldEqual(customerD);
        }
    }
}
