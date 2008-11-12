using Northwind.Core;
using FluentNHibernate.Mapping;

namespace Northwind.Data.NHibernateMappings
{
    public class OrderMap : ClassMap<Order>
    {
        public OrderMap() {
            WithTable("Orders");

            Id(x => x.ID, "OrderID")
                .WithUnsavedValue(0)
                .GeneratedBy.Identity();

            Map(x => x.OrderDate);
            Map(x => x.ShipToName, "ShipName");

            References(x => x.OrderedBy, "CustomerID")
                .SetAttribute("not-null", "true");
        }
    }
}
