using FluentNHibernate.AutoMap;
using Northwind.Core.Organization;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;
using FluentNHibernate.AutoMap.Alterations;

namespace Northwind.Data.NHibernateMappings
{
    public class TerritoryMap : IAutoMappingOverride<Territory>
    {
        public void Override(AutoMap<Territory> mapping) {
            // Evil assigned ID - use identity instead unless you're working with a legacy DB
            mapping.Id(x => x.Id, "TerritoryID")
                .GeneratedBy.Assigned();

            mapping.Map(x => x.Description, "TerritoryDescription");

            mapping.References(x => x.RegionBelongingTo, "RegionID")
                .SetAttribute("not-null", "true");

            mapping.HasManyToMany<Employee>(x => x.Employees)
                .WithTableName("EmployeeTerritories")
                .Inverse()
                .WithParentKeyColumn("TerritoryID")
                .WithChildKeyColumn("EmployeeID")
                .AsBag();
        }
    }
}
