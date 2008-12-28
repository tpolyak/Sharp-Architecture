using System.Reflection;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace SharpArch.Core
{
    /// <summary>
    /// Provides a standard base class for facilitating comparison of objects.
    /// 
    /// For a discussion of the implementation of Equals/GetHashCode, see 
    /// http://devlicio.us/blogs/billy_mccafferty/archive/2007/04/25/using-equals-gethashcode-effectively.aspx
    /// and http://groups.google.com/group/sharp-architecture/browse_thread/thread/f76d1678e68e3ece?hl=en for 
    /// an in depth and conclusive resolution.  Also provides property validation capabilities.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseObject
    {
        public override bool Equals(object obj) {
            BaseObject compareTo = obj as BaseObject;

            if (ReferenceEquals(this, compareTo))
                return true;

            return compareTo != null && GetType().Equals(compareTo.GetType()) &&
                HasSameObjectSignatureAs(compareTo);
        }

        /// <summary>
        /// Used to provide the hashcode identifier of an object using the signature 
        /// properties of the object; although it's necessary for NHibernate's use, this can 
        /// also be useful for business logic purposes and has been included in this base 
        /// class, accordingly.  Since it is recommended that GetHashCode change infrequently, 
        /// if at all, in an object's lifetime; it's important that properties are carefully
        /// selected which truly represent the signature of an object.
        /// </summary>
        public override int GetHashCode() {
            unchecked {
                // It's possible for two objects to return the same hash code based on 
                // identically valued properties, even if they're of two different types, 
                // so we include the object's type in the hash calculation
                int hashCode = GetType().GetHashCode();

                foreach (PropertyInfo property in SignatureProperties) {
                    object value = property.GetValue(this, null);

                    if (value != null)
                        hashCode = (hashCode * RANDOM_PRIME_NUMBER) ^ value.GetHashCode();
                }

                if (SignatureProperties.Any())
                    return hashCode;

                // If no properties were flagged as being part of the signature of the object,
                // then simply return the hashcode of the base object as the hashcode.
                return base.GetHashCode();
            }
        }

        /// <summary>
        /// You may override this method to provide your own comparison routine.
        /// </summary>
        protected virtual bool HasSameObjectSignatureAs(BaseObject compareTo) {
            foreach (PropertyInfo property in SignatureProperties) {
                object valueOfThisObject = property.GetValue(this, null);
                object valueToCompareTo = property.GetValue(compareTo, null);

                if (valueOfThisObject == null && valueToCompareTo == null)
                    continue;

                if ((valueOfThisObject == null ^ valueToCompareTo == null) ||
                    (!valueOfThisObject.Equals(valueToCompareTo))) {
                    return false;
                }
            }

            // If we've gotten this far and signature properties were found, then we can
            // assume that everything matched; otherwise, if there were no signature 
            // properties, then simply return the default bahavior of Equals
            return SignatureProperties.Any() || base.Equals(compareTo);
        }

        /// <summary>
        /// Enforces the template method pattern to have child objects determine which properties 
        /// should and should not be included in the object signature comparison.
        /// </summary>
        public abstract IEnumerable<PropertyInfo> SignatureProperties { get; }

        /// <summary>
        /// This particular magic number is often used in GetHashCode computations but is actually 
        /// quite random.  Resharper uses 397 as its number when overrideing GetHashCode, so it 
        /// either started there or has a deeper and more profound history than 42.
        /// </summary>
        private const int RANDOM_PRIME_NUMBER = 397;
    }
}
