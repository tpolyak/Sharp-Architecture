using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core;

namespace Northwind.Core
{
    /// <summary>
    /// I'd like to be perfectly clear that I think assigned IDs are almost always a terrible
    /// idea; this is a major complaint I have with the Northwind database.  With that said, 
    /// some legacy databases require such techniques.
    /// </summary>
    public class Customer : PersistentObjectWithTypedId<string>, IHasAssignedId<string>
    {
        /// <summary>
        /// Needed by ORM for reflective creation.
        /// </summary>
        protected Customer() {
            InitMembers();
        }

        public Customer(string companyName) {
            Check.Require(!string.IsNullOrEmpty(companyName), "companyName must be supplied");

            InitMembers();
            CompanyName = companyName;
        }

        /// <summary>
        /// Since we want to leverage automatic properties, init appropriate members here.
        /// </summary>
        private void InitMembers() {
            Orders = new List<Order>();
        }

        [DomainSignature]
        public string CompanyName { get; set; }

        [DomainSignature]
        public string ContactName { get; set; }
        
        public string Country { get; set; }
        public string Fax { get; set; }

        /// <summary>
        /// Note the protected set...only the ORM should set the collection reference directly
        /// after it's been initialized in <see cref="InitMembers" />
        /// </summary>
        public IList<Order> Orders { get; protected set; }

        public void SetAssignedIdTo(string assignedId) {
            Check.Require(!string.IsNullOrEmpty(assignedId), "assignedId may not be null or empty");
            Check.Require(assignedId.Trim().Length == 5, "assignedId must be exactly 5 characters");

            ID = assignedId.Trim().ToUpper();
        }
    }
}
