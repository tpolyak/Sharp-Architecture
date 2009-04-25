using System.Web.Mvc;
using Northwind.Core.Organization;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core.DomainModel;
using System.Collections.Generic;
using System;
using SharpArch.Web.NHibernate;
using NHibernate.Validator.Engine;
using System.Text;
using SharpArch.Web.CommonValidator;
using SharpArch.Core;

namespace Northwind.Web.Controllers.Organization
{
    [HandleError]
    public class EmployeesController : Controller
    {
        public EmployeesController(IRepository<Employee> employeeRepository) {
            Check.Require(employeeRepository != null, "employeeRepository may not be null");

            this.employeeRepository = employeeRepository;
        }

        /// <summary>
        /// The transaction on this action is optional, but recommended for performance reasons
        /// </summary>
        [Transaction]
        public ActionResult Index() {
            IList<Employee> employees = employeeRepository.GetAll();
            return View(employees);
        }

        /// <summary>
        /// The transaction on this action is optional, but recommended for performance reasons
        /// </summary>
        [Transaction]
        public ActionResult Show(int id) {
            Employee employee = employeeRepository.Get(id);
            return View(employee);
        }

        public ActionResult Create() {
            EmployeeFormViewModel viewModel = EmployeeFormViewModel.CreateEmployeeFormViewModel();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]   // Limits the method to only accept post requests
        public ActionResult Create(Employee employee) {
            if (ViewData.ModelState.IsValid && employee.IsValid()) {
                employeeRepository.SaveOrUpdate(employee);

                TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = 
					"The employee was successfully created.";
                return RedirectToAction("Index");
            }

            EmployeeFormViewModel viewModel = EmployeeFormViewModel.CreateEmployeeFormViewModel();
            viewModel.Employee = employee;
            return View(viewModel);
        }

        /// <summary>
        /// The transaction on this action is optional, but recommended for performance reasons
        /// </summary>
        [Transaction]
        public ActionResult Edit(int id) {
            EmployeeFormViewModel viewModel = EmployeeFormViewModel.CreateEmployeeFormViewModel();
            viewModel.Employee = employeeRepository.Get(id);
            return View(viewModel);
        }

        /// <summary>
        /// Accepts the form submission to update an existing item. This uses 
        /// <see cref="SharpModelBinder" /> to bind the model from the form.
        /// </summary>
        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Employee employee) {
            Employee employeeToUpdate = employeeRepository.Get(employee.Id);
            TransferFormValuesTo(employeeToUpdate, employee);

            if (ViewData.ModelState.IsValid && employee.IsValid()) {
                TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = 
					"The employee was successfully updated.";
                return RedirectToAction("Index");
            }
            else {
                employeeRepository.DbContext.RollbackTransaction();

				EmployeeFormViewModel viewModel = EmployeeFormViewModel.CreateEmployeeFormViewModel();
				viewModel.Employee = employee;
				return View(viewModel);
            }
        }

        private void TransferFormValuesTo(Employee employeeToUpdate, Employee employeeFromForm) {
			employeeToUpdate.FirstName = employeeFromForm.FirstName;
			employeeToUpdate.LastName = employeeFromForm.LastName;
			employeeToUpdate.PhoneExtension = employeeFromForm.PhoneExtension;
        }

        /// <summary>
        /// As described at http://stephenwalther.com/blog/archive/2009/01/21/asp.net-mvc-tip-46-ndash-donrsquot-use-delete-links-because.aspx
        /// there are a lot of arguments against doing a delete via a GET request.  This addresses that, accordingly.
        /// </summary>
        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id) {
            string resultMessage = "The employee was successfully deleted.";
            Employee employeeToDelete = employeeRepository.Get(id);

            if (employeeToDelete != null) {
                employeeRepository.Delete(employeeToDelete);

                try {
                    employeeRepository.DbContext.CommitChanges();
                }
                catch {
                    resultMessage = "A problem was encountered preventing the employee from being deleted. " +
						"Another item likely depends on this employee.";
                    employeeRepository.DbContext.RollbackTransaction();
                }
            }
            else {
                resultMessage = "The employee could not be found for deletion. It may already have been deleted.";
            }

            TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = resultMessage;
            return RedirectToAction("Index");
        }

		/// <summary>
		/// Holds data to be passed to the Employee form for creates and edits
		/// </summary>
        public class EmployeeFormViewModel
        {
            private EmployeeFormViewModel() { }

			/// <summary>
			/// Creation method for creating the view model. Services may be passed to the creation 
			/// method to instantiate items such as lists for drop down boxes.
			/// </summary>
            public static EmployeeFormViewModel CreateEmployeeFormViewModel() {
                EmployeeFormViewModel viewModel = new EmployeeFormViewModel();
                
                return viewModel;
            }

            public Employee Employee { get; internal set; }
        }

        private readonly IRepository<Employee> employeeRepository;
    }
}
