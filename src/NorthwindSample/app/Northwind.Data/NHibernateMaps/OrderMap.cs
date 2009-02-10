using FluentNHibernate.AutoMap;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;

namespace Northwind.Data.NHibernateMappings
{
    public class OrderMap : IAutoPeristenceModelConventionOverride
    {
        public AutoPersistenceModel Override(AutoPersistenceModel model) {
            return model.ForTypesThatDeriveFrom<Order>(map => {
                map.Id(x => x.ID, "OrderID")
                    .WithUnsavedValue(0)
                    .GeneratedBy.Identity();

                map.Map(x => x.OrderDate);
                map.Map(x => x.ShipToName, "ShipName");

                map.References(x => x.OrderedBy, "CustomerID")
                    .SetAttribute("not-null", "true");
            });
        }
    }
}
