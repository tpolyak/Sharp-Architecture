using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpArch.Core.PersistenceSupport;
using Northwind.Core;
using Northwind.Data;
using System.Diagnostics;
using SharpArch.Core;
using NUnit.Framework.SyntaxHelpers;
using SharpArch.Data.NHibernate;

namespace Tests.Northwind.Data
{
    [TestFixture]
    [Category("DB Tests")]
    public class CategoryDaoTests : DaoTests
    {
        [Test]
        public void CanGetAllCategories() {
            IList<Category> categories = categoryDao.LoadAll();

            Assert.That(categories, Is.Not.Null);
            Assert.That(categories, Is.Not.Empty);
        }

        [Test]
        public void CanGetCategoryById() {
            Category category = categoryDao.Load(1);

            Assert.That(category.Name, Is.EqualTo("Beverages"));
        }

        private IDao<Category> categoryDao = new GenericDao<Category>();
    }
}
