using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace $safeprojectname$.NHibernateMaps.Conventions {
    public class HasManyToManyConvention : IHasManyToManyConvention
    {
        public void Apply(IManyToManyCollectionInstance instance) {   
            instance.Cascade.SaveUpdate();
        }
    }
}
