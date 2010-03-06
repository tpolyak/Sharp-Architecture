using Northwind.Core;
using FluentNHibernate;
using FluentNHibernate.Mapping;
using SharpArch.Data.NHibernate.FluentNHibernate;

namespace Northwind.Data.NHibernateMappings
{
    public class CategoryMap : ClassMap<Category>, IMapGenerator
    {
        public CategoryMap() {
            WithTable("Categories");

            Id(x => x.ID, "CategoryID")
                .WithUnsavedValue(0)
                .GeneratedBy.Identity();
            
            Map(x => x.Name, "CategoryName");
        }

        #region IMapGenerator Members

        public System.Xml.XmlDocument Generate() {
            return CreateMapping(new MappingVisitor());
        }

        #endregion
    }
}
