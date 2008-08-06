using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Northwind.Core.DataInterfaces;
using Northwind.Core;
using NHibernate;
using ProjectBase.Data.NHibernate;
using NHibernate.Criterion;

namespace Northwind.Data
{
    public class CustomerDao : GenericDaoWithTypedId<Customer, string>, ICustomerDao
    {
        public List<Customer> FindByCountry(string countryName) {
            ICriteria criteria = Session.CreateCriteria(typeof(Customer))
                .Add(Expression.Eq("Country", countryName));

            return criteria.List<Customer>() as List<Customer>;
        }
    }
}
