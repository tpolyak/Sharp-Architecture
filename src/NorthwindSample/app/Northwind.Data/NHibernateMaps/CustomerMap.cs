using FluentNHibernate.AutoMap;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;
using FluentNHibernate.AutoMap.Alterations;

namespace Northwind.Data.NHibernateMappings
{
    public class CustomerMap : IAutoMappingOverride<Customer>
    {
        public void Override(AutoMap<Customer> mapping) {
            mapping.SetAttribute("lazy", "false");
        }
    }
}
