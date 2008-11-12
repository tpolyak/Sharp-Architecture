using Northwind.Core;
using FluentNHibernate.Mapping;

namespace Northwind.Data.NHibernateMappings
{
    public class RegionMap : ClassMap<Region>
    {
        public RegionMap() {
            // Why they didn't make this plural, when every other table is, is beyond me
            WithTable("Region");
            // This seems to be a reference type in Northwind, so let's make it immutable
            SetAttribute("mutable", "false");

            Id(x => x.ID, "RegionID")
                .WithUnsavedValue(0)
                .GeneratedBy.Increment();

            Map(x => x.Description, "RegionDescription");
        }
    }
}
