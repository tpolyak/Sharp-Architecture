using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using Northwind.Core;

namespace Northwind.Data.NHibernateMaps.Conventions
{
    public class TableNameConvention : IClassConvention
    {
        public bool Accept(IClassMap classMap) {
            // There's a class level exclusion here, but you can also create an override for the class
            // itself discussed at http://groups.google.com/group/fluent-nhibernate/browse_thread/thread/b0fbb7988b904028
            return classMap.EntityType != typeof(Region);
        }

        public void Apply(IClassMap classMap) {
            classMap.WithTable(Inflector.Net.Inflector.Pluralize(classMap.EntityType.Name));
        }
    }
}
