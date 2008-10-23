using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;

namespace Northwind.Core
{
    public class Region : PersistentObject
    {
        /// <summary>
        /// The Northwind DB doesn't make the ID of this object an identity field; consequently, 
        /// I'm making this a read-only reference object which can only get loaded via the ORM.
        /// NOTE: Not using an identity setting on the DB was a bad design decision for Northwind - learn from their mistakes!
        /// </summary>
        protected Region() { }

        [DomainSignature]
        public virtual string Description { get; protected set; }
    }
}
