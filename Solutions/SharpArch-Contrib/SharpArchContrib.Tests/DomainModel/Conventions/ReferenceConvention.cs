using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Tests.DomainModel.Conventions {
    public class ReferenceConvention : IReferenceConvention {
        #region IReferenceConvention Members

        [CLSCompliant(false)]
        public void Apply(IManyToOneInstance instance) {
            instance.Column(instance.Property.Name + "Id");
        }

        #endregion
    }
}