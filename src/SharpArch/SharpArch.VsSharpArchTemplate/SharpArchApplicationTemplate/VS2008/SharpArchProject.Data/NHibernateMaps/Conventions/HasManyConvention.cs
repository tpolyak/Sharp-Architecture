using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;

namespace $safeprojectname$.NHibernateMaps.Conventions {
    public class HasManyConvention : IHasManyConvention {
        public void Apply(FluentNHibernate.Conventions.Instances.IOneToManyCollectionInstance instance)
        {
            instance.Key.Column(instance.EntityType.Name + "Fk");
        }
    }
}
