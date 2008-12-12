using SharpArch.Core.PersistenceSupport;
using SharpArch.Core;
using NHibernate.Validator;

namespace Northwind.Core
{
    public class Product : PersistentObject
    {
        public Product() { }

        /// <summary>
        /// Creates valid domain object
        /// </summary>
        public Product(string name, Supplier supplier) {
            Supplier = supplier;
            Name = name;
        }

        [DomainSignature]
        [NotNullNotEmpty]
        public virtual string Name { get; set; }

        [DomainSignature]
        [NotNull]
        public virtual Supplier Supplier { get; protected set; }

        public virtual Category Category { get; set; }
    }
}
