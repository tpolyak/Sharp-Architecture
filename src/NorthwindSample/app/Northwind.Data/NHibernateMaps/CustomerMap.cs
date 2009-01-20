using FluentNHibernate.AutoMap;
using Northwind.Core;

namespace Northwind.Data.NHibernateMappings
{
    public class CustomerMap : AutoMap<Customer>
    {
        public CustomerMap() {
            SetAttribute("lazy", "false");
        }
    }
}
