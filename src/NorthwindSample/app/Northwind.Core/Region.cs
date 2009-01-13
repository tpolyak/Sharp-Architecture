using SharpArch.Core.DomainModel;
using SharpArch.Core.PersistenceSupport;
using NHibernate.Validator;
using SharpArch.Core;

namespace Northwind.Core
{
    public class Region : Entity, IHasAssignedId<int>
    {
        /// <summary>
        /// The Northwind DB doesn't make the ID of this object an identity field; 
        /// not using an identity setting on the DB was a bad design decision for 
        /// Northwind - learn from their mistakes!
        /// </summary>
        protected Region() { }

        public Region(string description) {
            Check.Require(!string.IsNullOrEmpty(description));
            Description = description;
        }

        [DomainSignature]
        public virtual string Description { get; protected set; }

        public virtual void SetAssignedIdTo(int assignedId) {
            ID = assignedId;
        }
    }
}
