using System.Collections.Generic;
using NUnit.Framework;
using SharpArch.Core.PersistenceSupport;
using Northwind.Core;
using SharpArch.Data.NHibernate;
using SharpArch.Testing.NUnit;
using SharpArch.Testing.NUnit.NHibernate;

namespace Tests.Northwind.Data
{
    [TestFixture]
    [Category("DB Tests")]
    public class CategoryRepositoryTests : RepositoryTestsBase
    {

        protected override void LoadTestData()
        {
            CreatePersistedCategory("Beverages");
            CreatePersistedCategory("Snacks");

        }

        [Test]
        public void CanGetAllCategories()
        {
            IList<Category> categories = categoryRepository.GetAll();

            categories.ShouldNotBeNull();
            categories.Count.ShouldEqual(2);
        }

        [Test]
        public void CanGetCategoryById()
        {
            Category category = categoryRepository.Get(1);

            category.CategoryName.ShouldEqual("Beverages");
        }

        private void CreatePersistedCategory(string categoryName)
        {
            Category category = new Category(categoryName);
            categoryRepository.SaveOrUpdate(category);
            FlushSessionAndEvict(category);
        }

        private IRepository<Category> categoryRepository = new Repository<Category>();
    }
}
