using FluentNHibernate.AutoMap;
using Northwind.Core.Organization;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;

namespace Northwind.Data.NHibernateMappings
{
    public class TerritoryMap : IAutoPeristenceModelConventionOverride
    {
        public AutoPersistenceModel Override(AutoPersistenceModel model) {
            return model.ForTypesThatDeriveFrom<Territory>(map => {
                // Evil assigned ID - use identity instead unless you're working with a legacy DB
                map.Id(x => x.ID, "TerritoryID")
                    .GeneratedBy.Assigned();

                map.Map(x => x.Description, "TerritoryDescription");

                map.References(x => x.RegionBelongingTo, "RegionID")
                    .SetAttribute("not-null", "true");

                map.HasManyToMany<Employee>(x => x.Employees)
                    .WithTableName("EmployeeTerritories")
                    .Inverse()
                    .WithParentKeyColumn("TerritoryID")
                    .WithChildKeyColumn("EmployeeID")
                    .AsBag();
            });
        }
    }
}
