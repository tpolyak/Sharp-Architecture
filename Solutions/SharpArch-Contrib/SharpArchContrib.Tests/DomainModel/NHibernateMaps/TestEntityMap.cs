using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Tests.DomainModel.Entities;

namespace Tests.DomainModel.NHibernateMaps {
    public class TestEntityMap : IAutoMappingOverride<TestEntity> {
        #region IAutoMappingOverride<TestEntity> Members

        [CLSCompliant(false)]
        public void Override(AutoMapping<TestEntity> mapping) {
            mapping.Map(c => c.Name).Unique();
        }

        #endregion
    }
}