using System.Web.Mvc;
using Northwind.Core;
using System.Collections.Generic;

namespace Northwind.Web.Views.Customers
{
    public partial class Index : ViewPage<IList<Customer>>
    {
        public void Page_Load() {
            customerList.DataSource = ViewData.Model;
            customerList.DataBind();
        }
    }
}
