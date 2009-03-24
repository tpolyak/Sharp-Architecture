using FluentNHibernate.AutoMap;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;
using FluentNHibernate.AutoMap.Alterations;

namespace Northwind.Data.NHibernateMappings
{
    public class OrderMap : IAutoMappingOverride<Order>
    {
        public void Override(AutoMap<Order> mapping) {
            mapping.Id(x => x.Id, "OrderID")
                .WithUnsavedValue(0)
                .GeneratedBy.Identity();

            mapping.Map(x => x.ShipToName, "ShipName");

            mapping.References(x => x.OrderedBy, "CustomerID")
                .SetAttribute("not-null", "true");
        }
    }
}
