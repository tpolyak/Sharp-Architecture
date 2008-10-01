using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core;

namespace Northwind.Core
{
    public class Supplier : PersistentObject
    {
        protected Supplier() {
            InitMembers();
        }

        public Supplier(string companyName) {
            InitMembers();

            CompanyName = companyName;
        }

        private void InitMembers() {
            Products = new List<Product>();
        }

        [DomainSignature]
        public virtual string CompanyName {
            get {
                return companyName;
            }
            set {
                Check.Require(!string.IsNullOrEmpty(value), "companyName must be provided");
                companyName = value;
            }
        }

        /// <summary>
        /// Note the protected set...only the ORM should set the collection reference directly
        /// after it's been initialized in <see cref="InitMembers" />
        /// </summary>
        public virtual IList<Product> Products { get; protected set; }

        private string companyName;
    }
}
