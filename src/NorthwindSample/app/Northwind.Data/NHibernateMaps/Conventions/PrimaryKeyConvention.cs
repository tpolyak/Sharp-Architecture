using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;

namespace Northwind.Data.NHibernateMaps.Conventions
{
    public class PrimaryKeyConvention : IIdConvention
    {
        public bool Accept(IIdentityPart id) {
            return true;
        }

        public void Apply(IIdentityPart id) {
            id.ColumnName(id.EntityType.Name + "ID");
                // We could include other details, such as those shown below, but the Northwind DB 
                // mixes up assigned Ids, generated Ids, string Ids and int Ids; thus, making it 
                // difficult to put in a single convention for this
                //.WithUnsavedValue(0)
                //.GeneratedBy.Identity();
        }
    }
}
