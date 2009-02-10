using FluentNHibernate.AutoMap;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;

namespace Northwind.Data.NHibernateMappings
{
    public class ProductMap : IAutoPeristenceModelConventionOverride
    {
        public AutoPersistenceModel Override(AutoPersistenceModel model) {
            return model.ForTypesThatDeriveFrom<Product>(map => {
                map.Id(x => x.ID, "ProductID")
                    .WithUnsavedValue(0)
                    .GeneratedBy.Identity();

                map.Map(x => x.ProductName);

                map.References(x => x.Supplier, "SupplierID");
                map.References(x => x.Category, "CategoryID");
            });
        }
    }
}
