using System.Web.Mvc;
using Northwind.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core;
using System.Collections.Generic;
using SharpArch.Web.NHibernate;
using System;

namespace Northwind.Controllers
{
    [HandleError]
    public class CategoriesController : Controller
    {
        public CategoriesController(IDao<Category> categoryDao) {
            Check.Require(categoryDao != null, "categoryDao may not be null");

            this.categoryDao = categoryDao;
        }

        public ActionResult Index() {
            List<Category> categories = categoryDao.LoadAll();
            return View(categories);
        }

        public ActionResult Show(int id) {
            Category category = categoryDao.Load(id);
            return View(category);
        }

        /// <summary>
        /// An example of creating an object with an auto incrementing ID.
        /// 
        /// Because this uses a declarative transaction, everything within this method is wrapped
        /// within a single transaction.
        /// </summary>
        [Transaction]
        public ActionResult Create(string categoryName) {
            Category category = new Category(categoryName);
            category = categoryDao.SaveOrUpdate(category);

            return View(category);
        }

        private readonly IDao<Category> categoryDao;
    }
}
