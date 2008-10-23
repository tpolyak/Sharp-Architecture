using SharpArch.Core.PersistenceSupport;
using SharpArch.Core;
using System.Collections.Generic;

namespace Northwind.Core
{
    /// <summary>
    /// The domain signature of this object isn't very realistic as you'll likely have same named 
    /// people in a large company.  Regardless, the Northwind DB doesn't provide a great domain 
    /// identifier, so the full name will have to do.  Alternatively, you don't have to have 
    /// domain signature properties.  If you don't, then Equals will use it's default behavior and
    /// compare the object references themselves.
    /// </summary>
    public class Employee : PersistentObject
    {
        /// <summary>
        /// This is a placeholder constructor for NHibernate.
        /// A no-argument constructor must be avilable for NHibernate to create the object.
        /// </summary>
        protected Employee() {
            InitMembers();
        }

        public Employee(string firstName, string lastName) {
            InitMembers();

            FirstName = firstName;
            LastName = lastName;
        }

        private void InitMembers() {
            // Init the collection so it's never null
            Territories = new List<Territory>();
        }

        [DomainSignature]
        public virtual string LastName {
            get {
                return lastName;
            }
            set {
                Check.Require(!string.IsNullOrEmpty(value));
                lastName = value;
            }
        }

        [DomainSignature]
        public virtual string FirstName {
            get {
                return firstName;
            }
            set {
                Check.Require(!string.IsNullOrEmpty(value));
                firstName = value;
            }
        }

        /// <summary>
        /// Note the protected set...only the ORM should set the collection reference directly
        /// after it's been initialized in <see cref="InitMembers" />
        /// </summary>
        public virtual IList<Territory> Territories { get; protected set; }

        private string firstName;
        private string lastName;
    }
}
