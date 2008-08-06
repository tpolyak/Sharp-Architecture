using Newtonsoft.Json;
using System;
using System.Text;
using System.Reflection;

namespace ProjectBase.Core.PersistenceSupport
{
    /// <summary>
    /// Facilitates indicating which property(s) of a class describe the unique signature of a 
    /// business object.  See PersistentObject.GetDomainObjectSignature for how this is leveraged.
    /// </summary>
    public class DomainSignatureAttribute : Attribute { }

    /// <summary>
    /// Since nearly all of the domain objects you create will have a type of int ID, this 
    /// most freqently used base PersistentObject leverages this assumption.  If you want a persistent 
    /// object with a type other than int, such as string, then use 
    /// <see cref="PersistentObjectwithTypedId" />.
    /// </summary>
    public abstract class PersistentObject : PersistentObjectWithTypedId<int> {}

    /// <summary>
    /// For a discussion of this object, see 
    /// http://devlicio.us/blogs/billy_mccafferty/archive/2007/04/25/using-equals-gethashcode-effectively.aspx
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class PersistentObjectWithTypedId<IdT>
    {
        /// <summary>
        /// ID may be of type string, int, custom type, etc.
        /// Setter is protected to allow unit tests to set this property via reflection and to allow 
        /// domain objects more flexibility in setting this for those objects with assigned IDs.
        /// It's virtual to allow NHibernate-backed objects to be lazily loaded.
        /// </summary>
        [JsonProperty]
        public virtual IdT ID {
            get { return id; }
            protected set { id = value; }
        }

        /// <summary>
        /// Transient objects are not associated with an item already in storage.  For instance,
        /// a <see cref="Customer" /> is transient if its ID is 0.  It's virtual to allow NHibernate-backed 
        /// objects to be lazily loaded.
        /// </summary>
        public virtual bool IsTransient() {
            return ID == null || ID.Equals(default(IdT));
        }

        public override bool Equals(object obj) {
            PersistentObjectWithTypedId<IdT> compareTo = obj as PersistentObjectWithTypedId<IdT>;

            return (compareTo != null) &&
                   (HasSameNonDefaultIdAs(compareTo) ||
                // Since the IDs aren't the same, either of them must be transient to 
                // compare business value signatures
                    (((IsTransient()) || compareTo.IsTransient()) &&
                     HasSameBusinessSignatureAs(compareTo)));
        }

        /// <summary>
        /// Must be provided to properly compare two objects.  This is sealed because the hash code
        /// is created using the result of <see cref="GetDomainObjectSignature" />.
        /// </summary>
        public override int GetHashCode() {
            return (GetType().FullName + "|" +
                    GetDomainObjectSignature()).GetHashCode();
        }

        /// <summary>
        /// The method GetDomainObjectSignature should ONLY return the "business value 
        /// signature" of the object and not the ID, which is handled by <see cref="PersistentObject" />.  
        /// The general structure of the overridden method should be as follows:  
        /// 
        /// return SomeProperty + "|" + SomeOtherProperty;
        /// 
        /// If you choose NOT to override this method, then you should decorate the appropriate 
        /// property(s) with [Identity] and they will be conatenated automatically.  Using the 
        /// [Identity] attribute is the preferred method and is only inapplicable with properties
        /// which may be nullable or which may have non-trivial business logic involved with creating 
        /// the domain signature.
        /// </summary>
        protected virtual string GetDomainObjectSignature() {
            DomainSignatureConcat domainSignatureConcat = new DomainSignatureConcat();

            foreach (PropertyInfo property in this.GetType().GetProperties()) {
                object[] attrs = property.GetCustomAttributes(typeof(DomainSignatureAttribute), true);
            
                if (attrs.Length > 0) {
                    object value = property.GetValue(this, null);
                    domainSignatureConcat.Apply(value);
                }
            }
            
            return domainSignatureConcat.Result;
        }

        private class DomainSignatureConcat
        {
            private bool isFirst = true;
            private StringBuilder sb = new StringBuilder();

            public string Result {
                get { 
                    return this.sb.ToString(); 
                } 
            }

            public void Apply(object value) {
                if (isFirst) {
                    isFirst = false;
                }
                else {
                    sb.Append('|');
                }

                if (value == null) {
                    sb.Append('*');
                }
                else {
                    sb.Append(value.GetHashCode());
                }
            }
        }

        private bool HasSameBusinessSignatureAs(PersistentObjectWithTypedId<IdT> compareTo) {
            Check.Require(compareTo != null, "compareTo may not be null");

            return GetHashCode().Equals(compareTo.GetHashCode());
        }

        /// <summary>
        /// Returns true if self and the provided persistent object have the same ID values 
        /// and the IDs are not of the default ID value
        /// </summary>
        private bool HasSameNonDefaultIdAs(PersistentObjectWithTypedId<IdT> compareTo) {
            Check.Require(compareTo != null, "compareTo may not be null");

            return (ID != null && !ID.Equals(default(IdT))) &&
                   (compareTo.ID != null && !compareTo.ID.Equals(default(IdT))) &&
                   ID.Equals(compareTo.ID);
        }

        private IdT id = default(IdT);
    }
}
