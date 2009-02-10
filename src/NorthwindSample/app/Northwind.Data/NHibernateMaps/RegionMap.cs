using FluentNHibernate.AutoMap;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;

namespace Northwind.Data.NHibernateMappings
{
    public class RegionMap : IAutoPeristenceModelConventionOverride
    {
        public AutoPersistenceModel Override(AutoPersistenceModel model) {
            return model.ForTypesThatDeriveFrom<Region>(map => {
                // Why they didn't make this plural, when every other table is, is beyond me
                map.WithTable("Region");
                // This seems to be a reference type in Northwind, so let's make it immutable
                map.SetAttribute("mutable", "false");

                map.Id(x => x.ID, "RegionID")
                    .WithUnsavedValue(0)
                    .GeneratedBy.Assigned();

                map.Map(x => x.Description, "RegionDescription");
            });
        }
    }
}
