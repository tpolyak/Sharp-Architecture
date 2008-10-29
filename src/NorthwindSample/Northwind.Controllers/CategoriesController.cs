using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SharpArch.Core.PersistenceSupport;
using Northwind.Core;
using SharpArch.Core;
using SharpArch.Web.NHibernate;

namespace Northwind.Controllers
{
    [HandleError]
    public class CategoriesController : Controller
    {
        public CategoriesController(IDao<Category> categoryDao) {
            Check.Require(categoryDao != null, "categoryDao may not be null");

            this.categoryDao = categoryDao;
        }

        public ActionResult List() {
            List<Category> categories = categoryDao.LoadAll();
            return View(categories);
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

        public ActionResult Detail(int id) {
            Check.Require(id > 0, "Category ID must be > 0");

            Category category = categoryDao.Load(id);
            return View(category);
        }

        private readonly IDao<Category> categoryDao;
    }
}
