using System.Collections.Generic;
using NUnit.Framework;
using SharpArch.Core.PersistenceSupport;
using Northwind.Core;
using Northwind.Data;
using System.Diagnostics;
using SharpArch.Core.DomainModel;
using SharpArch.Data.NHibernate;
using SharpArch.Testing.NUnit.NHibernate;

namespace Tests.Northwind.Data
{
    [TestFixture]
    [Category("DB Tests")]
    public class CategoryRepositoryTests : DatabaseRepositoryTestsBase
    {
        [Test]
        public void CanGetAllCategories() {
            IList<Category> categories = categoryRepository.GetAll();

            Assert.That(categories, Is.Not.Null);
            Assert.That(categories, Is.Not.Empty);
        }

        [Test]
        public void CanGetCategoryById() {
            Category category = categoryRepository.Get(1);

            Assert.That(category.CategoryName, Is.EqualTo("Beverages"));
        }

        private IRepository<Category> categoryRepository = new Repository<Category>();
    }
}
