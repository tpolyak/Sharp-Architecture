using Ninject.Core;
using SharpArch.Core.PersistenceSupport;
using Northwind.Core;
using SharpArch.Data.NHibernate;
using Northwind.Core.DataInterfaces;
using Northwind.Data;

namespace Northwind.Web.NinjectModules
{
    public class DataModule : StandardModule
    {
        public override void Load() {
            Bind<IRepository<Category>>().To<Repository<Category>>();
            Bind<IRepository<Employee>>().To<Repository<Employee>>();
            Bind<ICustomerRepository>().To<CustomerRepository>();
        }
    }
}
