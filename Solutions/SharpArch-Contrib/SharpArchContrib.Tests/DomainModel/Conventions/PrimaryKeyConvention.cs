using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Tests.DomainModel.Conventions {
    public class PrimaryKeyConvention : IIdConvention {
        #region IIdConvention Members

        [CLSCompliant(false)]
        public void Apply(IIdentityInstance instance) {
            instance.Column(instance.EntityType.Name + "Id");
            instance.UnsavedValue("0");
            instance.GeneratedBy.Identity();
        }

        #endregion
    }
}