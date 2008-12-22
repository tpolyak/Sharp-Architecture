using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Diagnostics;

namespace SharpArch.Core.PersistenceSupport
{
    /// <summary>
    /// Provides an optional base class for your objects which will be persisted to the database.
    /// Benefits include the addition of an ID property along with a consistent manner for comparing
    /// persistent objects.  If you're looking for a consistent means of comparing objects but don't
    /// need the ID property; consider inheriting from <see cref="DomainObject"/>, this object's base class.
    /// 
    /// Since nearly all of the persistent objects you create will have a type of int ID, this 
    /// base class leverages this assumption.  If you want a persistent object with a type other 
    /// than int, such as string, then use <see cref="PersistentObjectWithTypedId{IdT}" /> instead.
    /// </summary>
    [Serializable]
    public abstract class PersistentObject : PersistentObjectWithTypedId<int> { }

    /// <summary>
    /// For a discussion of this object, see 
    /// http://devlicio.us/blogs/billy_mccafferty/archive/2007/04/25/using-equals-gethashcode-effectively.aspx
    /// </summary>
    [Serializable]
    public abstract class PersistentObjectWithTypedId<IdT> : DomainObject, IPersistentObjectWithTypedId<IdT>
    {
        /// <summary>
        /// ID may be of type string, int, custom type, etc.
        /// Setter is protected to allow unit tests to set this property via reflection and to allow 
        /// domain objects more flexibility in setting this for those objects with assigned IDs.
        /// It's virtual to allow NHibernate-backed objects to be lazily loaded.
        /// </summary>
        [JsonProperty]
        public virtual IdT ID { get; protected set; }

        /// <summary>
        /// Transient objects are not associated with an item already in storage.  For instance,
        /// a Customer is transient if its ID is 0.  It's virtual to allow NHibernate-backed 
        /// objects to be lazily loaded.
        /// </summary>
        public virtual bool IsTransient() {
            return ID == null || ID.Equals(default(IdT));
        }

        public override bool Equals(object obj) {
            PersistentObjectWithTypedId<IdT> compareTo = obj as PersistentObjectWithTypedId<IdT>;

            if (ReferenceEquals(this, compareTo))
                return true;

            if (compareTo == null || !GetType().Equals(compareTo.GetType()))
                return false;

            if (HasSameNonDefaultIdAs(compareTo))
                return true;

            // Since the IDs aren't the same, both of them must be transient to 
            // compare domain signatures; because if one is transient and the 
            // other is a persisted entity, then they cannot be the same object.
            return IsTransient() && compareTo.IsTransient() &&
                HasSameObjectSignatureAs(compareTo);
        }

        /// <summary>
        /// Simply here to keep the compiler from complaining.
        /// </summary>
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns true if self and the provided persistent object have the same ID values 
        /// and the IDs are not of the default ID value
        /// </summary>
        private bool HasSameNonDefaultIdAs(PersistentObjectWithTypedId<IdT> compareTo) {
            return ! IsTransient() &&
                  ! compareTo.IsTransient() &&
                  ID.Equals(compareTo.ID);
        }
    }
}
