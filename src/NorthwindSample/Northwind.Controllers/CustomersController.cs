using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SharpArch.Core.PersistenceSupport;
using Northwind.Core;
using SharpArch.Core;
using Northwind.Core.DataInterfaces;
using SharpArch.Web.NHibernate;

namespace Northwind.Controllers
{
    public class CustomersController : Controller
    {
        public CustomersController(ICustomerDao customerDao) {
            Check.Require(customerDao != null, "customerDao may not be null");

            this.customerDao = customerDao;
        }

        public ActionResult List() {
            List<Customer> customers = customerDao.FindByCountry("Venezuela");
            return View(customers);
        }

        /// <summary>
        /// An example of creating an object with an assigned ID.  Because this uses a declarative 
        /// transaction, everything within this method is wrapped within a single transaction.
        /// 
        /// I'd like to be perfectly clear that I think assigned IDs are almost always a terrible
        /// idea; this is a major complaint I have with the Northwind database.  With that said, 
        /// some legacy databases require such techniques.
        /// </summary>
        [Transaction]
        public ActionResult Create(string companyName, string assignedId) {
            Customer customer = new Customer(companyName);
            customer.SetAssignedIdTo(assignedId);
            customerDao.Save(customer);

            return View(customer);
        }

        private readonly ICustomerDao customerDao;
    }
}
