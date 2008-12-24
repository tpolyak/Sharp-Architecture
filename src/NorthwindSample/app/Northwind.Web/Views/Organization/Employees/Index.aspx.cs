using System.Web.Mvc;
using Northwind.Core.Organization;
using System.Collections.Generic;

namespace Northwind.Web.Views.Organization.Employees
{
    public partial class Index : ViewPage<IEnumerable<Employee>>
    {
        public void Page_Load() {
            employeeList.DataSource = ViewData.Model;
            employeeList.DataBind();
        }
    }
}
