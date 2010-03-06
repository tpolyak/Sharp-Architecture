using NUnit.Framework;
using Northwind.Core;
using SharpArch.Testing.NUnit;

namespace Tests.Northwind.Core
{
    [TestFixture]
    public class ProductTests
    {
        [Test]
        public void CanCreateProduct() {
            Supplier supplierOfProduct = new Supplier("Acme");
            Product product = new Product("Fruit", supplierOfProduct);

            Category categoryOfProduct = new Category("Good Stuff");
            product.Category = categoryOfProduct;

            product.ProductName.ShouldEqual("Fruit");
            product.Supplier.ShouldEqual(new Supplier("Acme"));
            product.Category.ShouldEqual(categoryOfProduct);
        }

        [Test]
        public void CanCompareProducts() {
            Supplier supplierA = new Supplier("A");
            Supplier supplierB = new Supplier("B");

            (new Product("1", supplierA)).ShouldNotEqual(new Product("1", supplierB));
            (new Product("1", supplierA)).ShouldNotEqual(new Product("2", supplierB));
            (new Product("1", supplierA)).ShouldEqual(new Product("1", supplierA));
        }
    }
}
