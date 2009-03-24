using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using Northwind.Core;

namespace Northwind.Data.NHibernateMaps.Conventions
{
    public class TableNameConvention : IClassConvention
    {
        public bool Accept(IClassMap classMap) {
            // We shouldn't have to ignore the region class explicitly since an override exists;
            // The FNH team is working on correcting this.
            return classMap.EntityType != typeof(Region);
        }

        public void Apply(IClassMap classMap) {
            classMap.WithTable(Inflector.Net.Inflector.Pluralize(classMap.EntityType.Name));
        }
    }
}
