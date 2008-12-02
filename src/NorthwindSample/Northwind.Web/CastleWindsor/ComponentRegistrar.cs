using Castle.Windsor;
using SharpArch.Core.PersistenceSupport.NHibernate;
using SharpArch.Data.NHibernate;
using SharpArch.Core.PersistenceSupport;
using Northwind.Core.DataInterfaces;
using Northwind.Data;

namespace Northwind.Web.CastleWindsor
{
    public class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container) {
            AddGenericRepositoriesTo(container);
            AddCustomRepositoriesTo(container);
        }

        private static void AddCustomRepositoriesTo(IWindsorContainer container) {
            container.AddComponent("customerRepository",
                typeof(ICustomerRepository), typeof(CustomerRepository));
            container.AddComponent("supplierRepository",
                typeof(ISupplierRepository), typeof(SupplierRepository));
        }

        private static void AddGenericRepositoriesTo(IWindsorContainer container) {
            container.AddComponent("repositoryType",
                typeof(IRepository<>), typeof(Repository<>));
            container.AddComponent("nhibernateRepositoryType",
                typeof(INHibernateRepository<>), typeof(Repository<>));
        }
    }
}
