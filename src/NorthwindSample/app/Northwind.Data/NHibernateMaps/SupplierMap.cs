using FluentNHibernate.AutoMap;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;
using FluentNHibernate.AutoMap.Alterations;

namespace Northwind.Data.NHibernateMappings
{
    public class SupplierMap : IAutoMappingOverride<Supplier>
    {
        public void Override(AutoMap<Supplier> mapping) {
            mapping.SetAttribute("lazy", "false");

            mapping.Id(x => x.Id, "SupplierID")
                .WithUnsavedValue(0)
                .GeneratedBy.Identity();

            mapping.Map(x => x.CompanyName);

            mapping.HasMany<Product>(x => x.Products)
                .Inverse()
                .WithKeyColumn("SupplierID")
                .AsBag();
        }
    }
}
