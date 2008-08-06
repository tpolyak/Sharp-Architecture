using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ProjectBase.Core.PersistenceSupport;
using Northwind.Core;
using Northwind.Data;
using System.Diagnostics;
using ProjectBase.Core;
using NUnit.Framework.SyntaxHelpers;
using ProjectBase.Data.NHibernate;

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

        private IDao<Category> categoryDao = new GenericDao<Category>();
    }
}
