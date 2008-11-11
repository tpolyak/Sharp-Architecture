using System.Web.Mvc;
using Northwind.Core;
using System.Collections.Generic;

namespace Northwind.Web.Views.Employees
{
    public partial class Index : ViewPage<IList<Employee>>
    {
        public void Page_Load() {
            employeeList.DataSource = ViewData.Model;
            employeeList.DataBind();
        }
    }
}
