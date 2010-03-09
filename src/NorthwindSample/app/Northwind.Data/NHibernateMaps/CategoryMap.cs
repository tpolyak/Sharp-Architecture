using Northwind.Core;
using FluentNHibernate.Mapping;

namespace Northwind.Data.NHibernateMappings
{
    public class CategoryMap : ClassMap<Category>
    {
        public CategoryMap() {
            Table("Categories");

            Id(x => x.Id, "CategoryID")
                .UnsavedValue(0)
                .GeneratedBy.Identity();
            
            Map(x => x.CategoryName, "CategoryName");
        }
    }
}
