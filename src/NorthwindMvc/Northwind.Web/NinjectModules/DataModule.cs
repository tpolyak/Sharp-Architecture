using Ninject.Core;
using ProjectBase.Core.PersistenceSupport;
using Northwind.Core;
using ProjectBase.Data.NHibernate;
using Northwind.Core.DataInterfaces;
using Northwind.Data;

namespace Northwind.Web.NinjectModules
{
    public class DataModule : StandardModule
    {
        public override void Load() {
            Bind<IDao<Category>>().To<GenericDao<Category>>();
            Bind<ICustomerDao>().To<CustomerDao>();
        }
    }
}
