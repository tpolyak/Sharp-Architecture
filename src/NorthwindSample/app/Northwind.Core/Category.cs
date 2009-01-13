using SharpArch.Core.PersistenceSupport;
using NHibernate.Validator;
using SharpArch.Core.DomainModel;

namespace Northwind.Core
{
    public class Category : Entity
    {
        public Category() { }

        /// <summary>
        /// Creates valid domain object
        /// </summary>
        public Category(string name) {
            Name = name;
        }

        [DomainSignature]
        [NotNullNotEmpty]
        public virtual string Name { get; protected set; }
    }
}
