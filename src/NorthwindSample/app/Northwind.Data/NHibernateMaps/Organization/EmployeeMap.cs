using FluentNHibernate.AutoMap;
using Northwind.Core.Organization;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;
using FluentNHibernate.AutoMap.Alterations;

namespace Northwind.Data.NHibernateMappings.Organization
{
    public class EmployeeMap : IAutoMappingOverride<Employee>
    {
        public void Override(AutoMap<Employee> mapping) {
            mapping.Id(x => x.Id, "EmployeeID")
                .WithUnsavedValue(0)
                .GeneratedBy.Identity();

            // No need to specify the column name when it's the same as the property name
            mapping.Map(x => x.FirstName);
            mapping.Map(x => x.LastName);
            mapping.Map(x => x.PhoneExtension, "Extension");

            mapping.HasManyToMany<Territory>(x => x.Territories)
                .WithTableName("EmployeeTerritories")
                .WithParentKeyColumn("EmployeeID")
                .WithChildKeyColumn("TerritoryID")
                .AsBag();
        }
    }
}
