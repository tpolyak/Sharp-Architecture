using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport;
using Northwind.Core;
using NUnit.Framework;
using SharpArch.Testing.NUnit;
using SharpArch.Testing.NUnit.NHibernate;
using SharpArch.Data.NHibernate;

namespace Tests.Northwind.Data
{
    /// <summary>
    /// Example of behavior driven unit testing.  See <see cref="CategoryRepositoryTests" /> for 
    /// the more simplistic version of these tests.  See http://flux88.com/blog/the-transition-from-tdd-to-bdd/
    /// for more information about behavior driven unit testing; Google can find plenty more sources
    /// for you as well.
    /// </summary>
    [TestFixture]
    [Category("DB Tests")]
    public class WhenGettingAllCategories : RepositoryBehaviorSpecificationTestsBase
    {
        protected override void EstablishContext() {
            CreatePersistedCategory("Beverages");
            CreatePersistedCategory("Condiments");
            CreatePersistedCategory("Seafood");            
        }

        protected override void Act() {
            categories = categoryRepository.GetAll();            
        }

        [Test]
        public void CategoriesListShouldNotBeNull() {
            categories.ShouldNotBeNull();
        }

        [Test]
        public void CategoriesCountShouldEqualNumberOfTestItems() {
            categories.Count.ShouldEqual(3);
        }

        private void CreatePersistedCategory(string categoryName) {
            Category category = new Category(categoryName);
            categoryRepository.SaveOrUpdate(category);
            FlushSessionAndEvict(category);
        }

        private IList<Category> categories;
        private IRepository<Category> categoryRepository = new Repository<Category>();
    }

    [TestFixture]
    [Category("DB Tests")]
    public class WhenGettingCategoryById : RepositoryBehaviorSpecificationTestsBase
    {
        protected override void EstablishContext() {
            // create test data
            toDb = GetPersistedCategory("Beverages");
            GetPersistedCategory("Condiments");
            GetPersistedCategory("Seafood");
        }

        protected override void Act() {

            fromDb = categoryRepository.Get(toDb.Id);
        }

        [Test]
        public void RetrievedCategoryNameShouldEqualPersistedCategoryName() {
            fromDb.CategoryName.ShouldEqual(toDb.CategoryName);
        }

        [Test]
        public void RetrievedCategoryShouldNotBeTheSameAsPersistedCategory() {
            fromDb.ShouldNotBeTheSameAs(toDb);
        }

        private Category GetPersistedCategory(string categoryName) {
            var category = new Category(categoryName);
            categoryRepository.SaveOrUpdate(category);
            FlushSessionAndEvict(category);
            return category;
        }

        private Category toDb;
        private Category fromDb;
        private IRepository<Category> categoryRepository = new Repository<Category>();

    }
}
