using NUnit.Framework;
using Northwind.Core;
using SharpArch.Testing.NUnit;

namespace Tests.Northwind.Core
{
    [TestFixture]
    public class SupplierTests
    {
        [Test]
        public void CanCreateSupplier() {
            Supplier supplier = new Supplier("Acme");

            supplier.CompanyName.ShouldEqual("Acme");
        }

        [Test]
        public void CanAssociateProductsWithSupplier() {
            Supplier supplier = new Supplier("Acme");
            supplier.Products.Add(new Product("Strawberries", supplier));
            supplier.Products.Add(new Product("Apples", supplier));

            supplier.Products.Count.ShouldEqual(2);
        }
    }
}
