using System.Web.Mvc;
using Northwind.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core;
using System.Collections.Generic;
using System;
using SharpArch.Web.NHibernate;
using NHibernate.Validator.Engine;
using System.Text;

namespace Northwind.Controllers
{
    [HandleError]
    public class EmployeesController : Controller
    {
        public EmployeesController(IDao<Employee> employeeDao) {
            Check.Require(employeeDao != null, "employeeDao may not be null");

            this.employeeDao = employeeDao;
        }

        public ActionResult Index() {
            List<Employee> employees = employeeDao.LoadAll();
            return View(employees);
        }

        public ActionResult Show(int id) {
            Employee employee = employeeDao.Get(id);
            return View(employee);
        }

        public ActionResult Create() {
            return View();
        }

        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Employee employee) {
            if (employee.IsValid()) {
                employeeDao.SaveOrUpdate(employee);

                TempData["message"] = employee.FullName + " was successfully created.";
                return RedirectToAction("Index");
            }
            else {
                TransferValidationMessagesToModelState(employee.ValidationMessages);
                return View();
            }
        }

        public ActionResult Edit(int id) {
            Employee employee = employeeDao.Get(id);
            return View(employee);
        }

        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Employee employee) {
            Employee employeeToUpdate = employeeDao.Get(id);

            if (employee.IsValid()) {
                employeeToUpdate.FirstName = employee.FirstName;
                employeeToUpdate.LastName = employee.LastName;
                employeeToUpdate.PhoneExtension = employee.PhoneExtension;

                TempData["message"] = employee.FullName + " was successfully updated.";
                return RedirectToAction("Index");
            }
            else {
                TransferValidationMessagesToModelState(employee.ValidationMessages);
                return View();
            }
        }

        private void TransferValidationMessagesToModelState(InvalidValue[] invalidValues) {
            foreach (InvalidValue invalidValue in invalidValues) {
                ViewData.ModelState.AddModelError(invalidValue.BeanClass.Name + "." +
                    invalidValue.PropertyName, invalidValue.Message);
            }
        }

        [Transaction]
        public ActionResult Delete(int id) {
            string resultMessage = null;
            Employee employeeToDelete = employeeDao.Get(id, Enums.LockMode.Upgrade);

            if (employeeToDelete != null) {
                employeeDao.Delete(employeeToDelete);

                try {
                    employeeDao.DbContext.CommitChanges();
                }
                catch {
                    resultMessage = "The employee couldn't be deleted; likely due to a foreign key " +
                        "reference. You could cascade the deletion or you could inform " +
                        "the user better on where the foreign dependencies are and what needs to be " +
                        "done before the employee can be deleted.";
                    employeeDao.DbContext.RollbackTransaction();
                }
            }
            else {
                resultMessage = "An employee with the ID of " + id + " could not be found for deletion.";
            }

            if (resultMessage == null) {
                resultMessage = "The employee was successfully deleted.";
            }

            TempData["Message"] = resultMessage;
            return RedirectToAction("Index");
        }

        private readonly IDao<Employee> employeeDao;
    }
}
