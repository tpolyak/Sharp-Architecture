using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Northwind.Core;

namespace Northwind.Web.Views.Categories
{
    public partial class List : ViewPage<IList<Category>>
    {
        public void Page_Load() {
            categoryList.DataSource = ViewData.Model;
            categoryList.DataBind();
        }
    }
}
