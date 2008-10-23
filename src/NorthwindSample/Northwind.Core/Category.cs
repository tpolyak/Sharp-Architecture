using SharpArch.Core.PersistenceSupport;
using SharpArch.Core;
using System;

namespace Northwind.Core
{
    public class Category : PersistentObject
    {
        protected Category() { }
        
        public Category(string name) {
            Check.Require(!String.IsNullOrEmpty(name), "name may not be null or empty");
            Name = name;
        }

        [DomainSignature]
        public virtual string Name { get; protected set; }
    }
}
