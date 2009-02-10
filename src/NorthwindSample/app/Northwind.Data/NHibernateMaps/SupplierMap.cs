using FluentNHibernate.AutoMap;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;

namespace Northwind.Data.NHibernateMappings
{
    public class SupplierMap : IAutoPeristenceModelConventionOverride
    {
        public AutoPersistenceModel Override(AutoPersistenceModel model) {
            return model.ForTypesThatDeriveFrom<Supplier>(map => {
                map.SetAttribute("lazy", "false");

                map.Id(x => x.ID, "SupplierID")
                    .WithUnsavedValue(0)
                    .GeneratedBy.Identity();

                map.Map(x => x.CompanyName);

                map.HasMany<Product>(x => x.Products)
                    .Inverse()
                    .WithKeyColumn("SupplierID")
                    .AsBag();
            });
        }
    }
}
