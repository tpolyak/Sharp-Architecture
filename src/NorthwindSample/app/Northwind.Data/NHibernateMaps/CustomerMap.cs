using FluentNHibernate.Automapping;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;
using FluentNHibernate.Automapping.Alterations;

namespace Northwind.Data.NHibernateMappings
{
    public class CustomerMap : IAutoMappingOverride<Customer>
    {
        public void Override(AutoMapping<Customer> mapping) {
            mapping.Not.LazyLoad();
        }
    }
}
