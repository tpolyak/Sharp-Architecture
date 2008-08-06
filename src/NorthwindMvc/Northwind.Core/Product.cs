using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectBase.Core.PersistenceSupport;
using ProjectBase.Core;

namespace Northwind.Core
{
    public class Product : PersistentObject
    {
        /// <summary>
        /// This is a placeholder constructor for NHibernate.
        /// A no-argument constructor must be avilable for NHibernate to create the object.
        /// </summary>
        protected Product() { }

        public Product(string name, Supplier supplier) {
            Check.Require(supplier != null, "supplier must be supplied");

            Supplier = supplier;
            Name = name;
        }

        [DomainSignature]
        public virtual string Name {
            get {
                return name;
            }
            set {
                Check.Require(!string.IsNullOrEmpty(value), "name must be supplied");
                name = value;
            }
        }

        public virtual Category Category { get; set; }

        [DomainSignature]
        public virtual Supplier Supplier { get; protected set; }

        private string name;
    }
}
