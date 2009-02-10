using FluentNHibernate.AutoMap;
using Northwind.Core.Organization;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;

namespace Northwind.Data.NHibernateMappings.Organization
{
    public class EmployeeMap : IAutoPeristenceModelConventionOverride
    {
        public AutoPersistenceModel Override(AutoPersistenceModel model) {
            return model.ForTypesThatDeriveFrom<Employee>(map => {
                map.Id(x => x.ID, "EmployeeID")
                    .WithUnsavedValue(0)
                    .GeneratedBy.Identity();

                // No need to specify column when it's the same as the property name
                map.Map(x => x.FirstName);
                map.Map(x => x.LastName);
                map.Map(x => x.PhoneExtension, "Extension");

                map.HasManyToMany<Territory>(x => x.Territories)
                    .WithTableName("EmployeeTerritories")
                    .WithParentKeyColumn("EmployeeID")
                    .WithChildKeyColumn("TerritoryID")
                    .AsBag();
            });
        }
    }
}
