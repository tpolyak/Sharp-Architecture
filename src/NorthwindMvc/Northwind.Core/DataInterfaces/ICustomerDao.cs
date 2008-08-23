using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectBase.Core.PersistenceSupport;
using ProjectBase.Core;

namespace Northwind.Core.DataInterfaces
{
    public interface ICustomerDao : IDaoWithTypedId<Customer, string>
    {
        List<Customer> FindByCountry(string countryName);
    }
}
