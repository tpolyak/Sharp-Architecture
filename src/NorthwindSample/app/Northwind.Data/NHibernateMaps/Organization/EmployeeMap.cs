using FluentNHibernate.Mapping;
using Northwind.Core.Organization;
using Northwind.Core;

namespace Northwind.Data.NHibernateMappings.Organization
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap() {
            WithTable("Employees");

            Id(x => x.ID, "EmployeeID")
                .WithUnsavedValue(0)
                .GeneratedBy.Identity();

            // No need to specify column when it's the same as the property name
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.PhoneExtension, "Extension");

            HasManyToMany<Territory>(x => x.Territories)
                .WithTableName("EmployeeTerritories")
                .WithParentKeyColumn("EmployeeID")
                .WithChildKeyColumn("TerritoryID")
                .AsBag();
        }
    }
}
