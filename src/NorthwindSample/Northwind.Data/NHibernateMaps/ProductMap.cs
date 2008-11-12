using Northwind.Core;
using FluentNHibernate.Mapping;

namespace Northwind.Data.NHibernateMappings
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap() {
            WithTable("Products");

            Id(x => x.ID, "ProductID")
                .WithUnsavedValue(0)
                .GeneratedBy.Identity();

            Map(x => x.Name, "ProductName");

            References(x => x.Supplier, "SupplierID");
            References(x => x.Category, "CategoryID");
        }
    }
}
