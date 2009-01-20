using FluentNHibernate.AutoMap;
using Northwind.Core;

namespace Northwind.Data.NHibernateMappings
{
    public class ProductMap : AutoMap<Product>
    {
        public ProductMap() {
            WithTable("Products");

            Id(x => x.ID, "ProductID")
                .WithUnsavedValue(0)
                .GeneratedBy.Identity();

            Map(x => x.ProductName, "ProductName");

            References(x => x.Supplier, "SupplierID");
            References(x => x.Category, "CategoryID");
        }
    }
}
