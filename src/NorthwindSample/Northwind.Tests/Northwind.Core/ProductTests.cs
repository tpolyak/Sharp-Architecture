using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Northwind.Core;

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

            Assert.That(product.Name, Is.EqualTo("Fruit"));
            Assert.That(product.Supplier, Is.EqualTo(new Supplier("Acme")));
            Assert.That(product.Category, Is.EqualTo(categoryOfProduct));
        }

        [Test]
        public void CanCompareProducts() {
            Supplier supplierA = new Supplier("A");
            Supplier supplierB = new Supplier("B");

            Assert.That(new Product("1", supplierA), 
                Is.Not.EqualTo(new Product("1", supplierB)));
            Assert.That(new Product("1", supplierA),
                Is.Not.EqualTo(new Product("2", supplierA)));
            Assert.That(new Product("1", supplierA),
                Is.EqualTo(new Product("1", supplierA)));
        }
    }
}
