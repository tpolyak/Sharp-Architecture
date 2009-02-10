using FluentNHibernate.AutoMap;
using Northwind.Core;
using SharpArch.Data.NHibernate.FluentNHibernate;

namespace Northwind.Data.NHibernateMappings
{
    public class CustomerMap : IAutoPeristenceModelConventionOverride
    {
        public AutoPersistenceModel Override(AutoPersistenceModel model) {
            return model.ForTypesThatDeriveFrom<Customer>(map => {
                map.SetAttribute("lazy", "false");
            });
        }
    }
}
