using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Northwind.Core.DataInterfaces;
using Northwind.Core;
using NHibernate;
using NHibernate.Transform;
using SharpArch.Data.NHibernate;
using NHibernate.Criterion;

namespace Northwind.Data
{
    public class SupplierDao : GenericDao<Supplier>, ISupplierDao
    {
        /// <summary>
        /// Uses NHibernate's CreateAlias to create a join query from the <see cref="Supplier" />
        /// to its collection of <see cref="Product" /> items to the category in which each
        /// product belongs.  This
        /// </summary>
        /// <remarks>Note that a category alias would not be necessary if we were trying to match the category ID.</remarks>
        public List<Supplier> LoadSuppliersBy(string productCategoryName) {
            ICriteria criteria = Session.CreateCriteria(typeof(Supplier))
                .CreateAlias("Products", "product")
                .CreateAlias("product.Category", "productCategory")
                .Add(Expression.Eq("productCategory.Name", productCategoryName))
                .SetResultTransformer(new DistinctRootEntityResultTransformer());

            return criteria.List<Supplier>() as List<Supplier>;
        }
    }
}
