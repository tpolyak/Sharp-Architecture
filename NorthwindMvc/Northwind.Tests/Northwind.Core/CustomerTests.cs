using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Northwind.Core;
using ProjectBase.Core;
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
        [ExpectedException(typeof(PreconditionException))]
        public void CannotCreateCustomerWithoutCompanyName() {
            new Customer("");
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
            // were the same.  Call me crazy, but I put that much trust into IDs.
            Assert.That(customerA, Is.EqualTo(customerB));

            Customer customerC = new Customer("Acme");

            // Since customerA has an ID but customerC doesn't, their signatures will be compared
            Assert.That(customerA, Is.EqualTo(customerC));

            customerC.ContactName = "Coyote";

            // Signatures are now different
            Assert.That(customerA, Is.Not.EqualTo(customerC));

            // customerA.Equals(customerB) because they have the same ID.
            // customerA.Equals(customerC) because they have the same signature.
            // ! customerB.Equals(customerC) because we can't compare their IDs, 
            // since customerC is transient, and their signatures are different.
            Assert.That(customerB, Is.Not.EqualTo(customerC));
        }
    }
}
