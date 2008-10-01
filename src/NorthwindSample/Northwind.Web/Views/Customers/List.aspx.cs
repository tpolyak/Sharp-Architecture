using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Northwind.Core;

namespace Northwind.Web.Views.Customers
{
    public partial class List : ViewPage<IList<Customer>>
    {
        public void Page_Load() {
            customerList.DataSource = ViewData.Model;
            customerList.DataBind();
        }
    }
}
