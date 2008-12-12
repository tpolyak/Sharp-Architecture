using SharpArch.Core;
using SharpArch.Core.PersistenceSupport.NHibernate;
using System.Collections.Generic;

namespace Northwind.Core.DataInterfaces
{
    /// <summary>
    /// Needs to implement INHibernateRepositoryWithTypedId because it has an assigned ID
    /// and will need to be explicit about called Save or Update appropriately.  Assigned
    /// IDs are EVil with a capital E and V...yes, they're just that evil.
    /// </summary>
    public interface ICustomerRepository : INHibernateRepositoryWithTypedId<Customer, string>
    {
        List<Customer> FindByCountry(string countryName);
    }
}
