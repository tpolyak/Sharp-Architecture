using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Northwind.Core;
using NUnit.Framework.SyntaxHelpers;
using ProjectBase.Core.PersistenceSupport;
using Northwind.Core.DataInterfaces;

namespace Tests.Northwind.Core
{
    [TestFixture]
    public class CategoryTests
    {
        [Test]
        public void CanCreateCategory() {
            string categoryName = "Just testing";
            Category category = new Category(categoryName);

            Assert.That(category.Name, Is.EqualTo(categoryName));
        }

        [Test]
        public void CanCompareCategories() {
            Category category1 = new Category("Acme");
            Category category2 = new Category("Anvil");
            Category category3 = new Category("Acme");

            Assert.That(category1, Is.Not.EqualTo(category2));
            Assert.That(category1.Equals(category3));
        }
    }
}
