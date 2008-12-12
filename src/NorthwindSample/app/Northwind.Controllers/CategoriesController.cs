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
        public CategoriesController(IRepository<Category> categoryRepository) {
            Check.Require(categoryRepository != null, "categoryRepository may not be null");

            this.categoryRepository = categoryRepository;
        }

        public ActionResult Index() {
            List<Category> categories = categoryRepository.GetAll();
            return View(categories);
        }

        public ActionResult Show(int id) {
            Category category = categoryRepository.Get(id);
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
            category = categoryRepository.SaveOrUpdate(category);

            return View(category);
        }

        private readonly IRepository<Category> categoryRepository;
    }
}
