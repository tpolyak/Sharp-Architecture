using Northwind.Core;
using FluentNHibernate.Mapping;

namespace Northwind.Data.NHibernateMappings
{
    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap() {
            WithTable("Customers");
            SetAttribute("lazy", "false");

            // Evil assigned ID - use identity instead unless you're working with a legacy DB
            Id(x => x.ID, "CustomerID")
                .GeneratedBy.Assigned();

            Map(x => x.CompanyName);
            Map(x => x.ContactName);
            Map(x => x.Country);
            Map(x => x.Fax);

            HasMany<Order>(x => x.Orders)
                .IsInverse()
                .WithKeyColumn("CustomerID")
                .AsBag();
        }
    }
}
