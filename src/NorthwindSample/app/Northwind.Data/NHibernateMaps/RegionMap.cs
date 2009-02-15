using FluentNHibernate.AutoMap;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;
using FluentNHibernate.AutoMap.Alterations;

namespace Northwind.Data.NHibernateMappings
{
    public class RegionMap : IAutoMappingOverride<Region>
    {
        public void Override(AutoMap<Region> mapping) {
            // Why they didn't make this plural, when every other table is, is beyond me
            mapping.WithTable("Region");
            // This seems to be a reference type in Northwind, so let's make it immutable
            mapping.SetAttribute("mutable", "false");

            mapping.Id(x => x.Id, "RegionID")
                .WithUnsavedValue(0)
                .GeneratedBy.Assigned();

            mapping.Map(x => x.Description, "RegionDescription");
        }
    }
}
