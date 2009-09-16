using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;

namespace Northwind.Data.NHibernateMaps.Conventions
{
    public class PrimaryKeyConvention : IIdConvention
    {
        public void Apply(FluentNHibernate.Conventions.Instances.IIdentityInstance instance)
        {
            instance.Column(instance.EntityType.Name + "ID");
        }
    }
}
