using Northwind.Core;
using FluentNHibernate.Mapping;

namespace Northwind.Data.NHibernateMappings
{
    public class SupplierMap : ClassMap<Supplier>
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
