using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;

namespace $safeprojectname$.NHibernateMaps.Conventions
{
    public class PrimaryKeyConvention : IIdConvention
    {
        public bool Accept(IIdentityPart id) {
            return true;
        }

        public void Apply(IIdentityPart id) {
            id.ColumnName("Id")
                .WithUnsavedValue(0)
                .GeneratedBy.Identity();
        }
    }
}
