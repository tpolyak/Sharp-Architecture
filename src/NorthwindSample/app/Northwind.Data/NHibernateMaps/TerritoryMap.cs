using Northwind.Core.Organization;
using FluentNHibernate.Mapping;
using Northwind.Core;

namespace Northwind.Data.NHibernateMappings
{
    public class TerritoryMap : ClassMap<Territory>
    {
        public TerritoryMap() {
            WithTable("Territories");

            // Evil assigned ID - use identity instead unless you're working with a legacy DB
            Id(x => x.ID, "TerritoryID")
                .GeneratedBy.Assigned();

            Map(x => x.Description, "TerritoryDescription");

            References(x => x.RegionBelongingTo, "RegionID")
                .SetAttribute("not-null", "true");

            HasManyToMany<Employee>(x => x.Employees)
                .WithTableName("EmployeeTerritories")
                .IsInverse()
                .WithParentKeyColumn("TerritoryID")
                .WithChildKeyColumn("EmployeeID")
                .AsBag();
        }
    }
}
