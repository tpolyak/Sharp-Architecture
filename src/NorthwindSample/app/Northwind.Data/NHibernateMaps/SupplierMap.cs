using FluentNHibernate.AutoMap;
using Northwind.Core;

namespace Northwind.Data.NHibernateMappings
{
    public class SupplierMap : AutoMap<Supplier>
    {
        public SupplierMap() {
            WithTable("Suppliers");
            SetAttribute("lazy", "false");

            Id(x => x.ID, "SupplierID")
                .WithUnsavedValue(0)
                .GeneratedBy.Identity();

            Map(x => x.CompanyName);

            HasMany<Product>(x => x.Products)
                .IsInverse()
                .WithKeyColumn("SupplierID")
                .AsBag();
        }
    }
}
