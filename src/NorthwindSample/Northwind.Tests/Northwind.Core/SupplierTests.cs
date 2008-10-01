using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Northwind.Core;

namespace Tests.Northwind.Core
{
    [TestFixture]
    public class SupplierTests
    {
        [Test]
        public void CanCreateSupplier() {
            Supplier supplier = new Supplier("Acme");

            Assert.That(supplier.CompanyName, Is.EqualTo("Acme"));
        }

        [Test]
        public void CanAssociateProductsWithSupplier() {
            Supplier supplier = new Supplier("Acme");
            supplier.Products.Add(new Product("Strawberries", supplier));
            supplier.Products.Add(new Product("Apples", supplier));

            Assert.That(supplier.Products.Count, Is.EqualTo(2));
        }
    }
}
