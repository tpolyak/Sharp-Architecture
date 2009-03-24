using FluentNHibernate.AutoMap;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;
using FluentNHibernate.AutoMap.Alterations;

namespace Northwind.Data.NHibernateMappings
{
    public class ProductMap : IAutoMappingOverride<Product>
    {
        public void Override(AutoMap<Product> mapping) {
            mapping.Id(x => x.Id, "ProductID")
                .WithUnsavedValue(0)
                .GeneratedBy.Identity();

            mapping.References(x => x.Supplier, "SupplierID");
            mapping.References(x => x.Category, "CategoryID");
        }
    }
}
