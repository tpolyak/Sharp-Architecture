using NUnit.Framework;
using Northwind.Core;
using SharpArch.Testing.NUnit;

namespace Tests.Northwind.Core
{
    [TestFixture]
    public class CategoryTests
    {
        [Test]
        public void CanCreateCategory() {
            string categoryName = "Just testing";
            Category category = new Category(categoryName);

            category.CategoryName.ShouldEqual(categoryName);
        }

        [Test]
        public void CanCompareCategories() {
            Category category1 = new Category("Acme");
            Category category2 = new Category("Anvil");
            Category category3 = new Category("Acme");

            category1.ShouldNotEqual(category2);
            category1.ShouldEqual(category3);
        }
    }
}
