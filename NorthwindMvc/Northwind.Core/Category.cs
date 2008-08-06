using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectBase.Core.PersistenceSupport;
using ProjectBase.Core;

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
