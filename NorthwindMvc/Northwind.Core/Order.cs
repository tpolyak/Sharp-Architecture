using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectBase.Core.PersistenceSupport;
using ProjectBase.Core;

namespace Northwind.Core
{
    public class Order : PersistentObject
    {
        /// <summary>
        /// This is a placeholder constructor for NHibernate.
        /// A no-argument constructor must be avilable for NHibernate to create the object.
        /// </summary>
        protected Order() { }

        public Order(Customer orderedBy) {
            Check.Require(orderedBy != null, "orderedBy may not be null");

            OrderedBy = orderedBy;
        }

        public virtual DateTime? OrderDate { get; set; }
        public virtual string ShipToName { get; set; }
        public virtual Customer OrderedBy { get; protected set; }

        /// <summary>
        /// Should ONLY contain the "business value signature" of the object and not the ID, 
        /// which is handled by <see cref="PersistentObject" />.  This method should return a unique, 
        /// usually pipe delimited string, representing a unique signature of the domain object.  For 
        /// example, no two different orders should have the same ShipToName, OrderDate and OrderedBy;
        /// therefore, the returned "signature" should be expressed as demonstrated below.
        /// 
        /// Alternatively, we could decorate properties with the [Identity] attribute, but since 
        /// there is conditional logic with respect to how to handle nullable types, GetDomainObjectSignature
        /// should be overridden instead.
        /// </summary>
        protected override string GetDomainObjectSignature() {
            return ShipToName + "|" +
                    (OrderDate ?? DateTime.MinValue).GetHashCode() + "|" +
                    OrderedBy.GetHashCode();
        }
    }
}
