using SharpArch.Core.PersistenceSupport;
using SharpArch.Core;
using System.Collections.Generic;

namespace Northwind.Core
{
    public class Territory : PersistentObjectWithTypedId<string>, IHasAssignedId<string>
    {
        /// <summary>
        /// This is a placeholder constructor for NHibernate.
        /// A no-argument constructor must be avilable for NHibernate to create the object.
        /// </summary>
        protected Territory() {
            InitMembers();
        }

        public Territory(string description, Region regionBelongingTo) {
            Check.Require(regionBelongingTo != null);
            // Don't need to Check.Require description as the property setter will take care of it

            InitMembers();

            RegionBelongingTo = regionBelongingTo;
            Description = description;
        }

        private void InitMembers() {
            // Init the collection so it's never null
            Employees = new List<Employee>();
        }

        /// <summary>
        /// Let me remind you that I completely disdane assigned IDs...another lesson to be learned
        /// from the fallacies of the Northwind DB.
        /// </summary>
        public virtual void SetAssignedIdTo(string assignedId) {
            Check.Require(!string.IsNullOrEmpty(assignedId) && assignedId.Length <= ID_MAX_LENGTH);
            ID = assignedId;
        }

        /// <summary>
        /// Let's assume that a territory can't move to a different region after it's created
        /// </summary>
        [DomainSignature]
        public virtual Region RegionBelongingTo { get; protected set; }
        
        /// <summary>
        /// We can't use automatic accessors here because we need a public setter for the 
        /// description but want to enforce our contract
        /// </summary>
        [DomainSignature]
        public virtual string Description {
            get {
                return description;
            }
            set {
                Check.Require(!string.IsNullOrEmpty(value));
                description = value;
            }
        }

        /// <summary>
        /// Note the protected set...only the ORM should set the collection reference directly
        /// after it's been initialized in <see cref="InitMembers" />
        /// </summary>
        public virtual IList<Employee> Employees { get; protected set; }

        private const int ID_MAX_LENGTH = 20;
        private string description;
    }
}
