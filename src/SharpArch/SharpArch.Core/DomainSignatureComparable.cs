using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace SharpArch.Core
{
    /// <summary>
    /// Facilitates indicating which property(s) of a class describe the unique signature of a 
    /// business object.  See PersistentObject.GetDomainObjectSignature for how this is leveraged.
    /// </summary>
    [Serializable]
    public class DomainSignatureAttribute : Attribute { }

    /// <summary>
    /// Provides a standard base class for facilitating comparison of domain objects via the 
    /// <see cref="DomainSignatureAttribute" />.
    /// 
    /// For a discussion of the implementation of Equals/GetHashCode, see 
    /// http://devlicio.us/blogs/billy_mccafferty/archive/2007/04/25/using-equals-gethashcode-effectively.aspx
    /// and http://groups.google.com/group/sharp-architecture/browse_thread/thread/f76d1678e68e3ece?hl=en for 
    /// an in depth and conclusive resolution.
    /// </summary>
    [Serializable]
    public abstract class DomainSignatureComparable
    {
        public override bool Equals(object obj) {
            DomainSignatureComparable compareTo = obj as DomainSignatureComparable;

            if (ReferenceEquals(this, compareTo))
                return true;

            return compareTo != null && GetType().Equals(compareTo.GetType()) &&
                HasSameDomainObjectSignatureAs(compareTo);
        }

        /// <summary>
        /// Used to provide the hashcode identifier of an object using the domain signature 
        /// properties of the object; although it's necessary for NHibernate's use, this can 
        /// also be useful for business logic purposes and has been included in this base 
        /// class, accordingly.  Since it is recommended that GetHashCode change infrequently, 
        /// if at all, in an object's lifetime; it's important that properties are carefully
        /// selected which truly represent the domain signature of an object.
        /// </summary>
        public override int GetHashCode() {
            unchecked {
                // It's possible for two objects to return the same hash code based on 
                // identically valued properties, even if they're of two different types, 
                // so we include the object's type in the hash calculation
                int hashCode = GetType().GetHashCode();

                IEnumerable<PropertyInfo> domainSignatureProperties = GetDomainSignatureProperties();

                foreach (PropertyInfo property in domainSignatureProperties) {
                    object value = property.GetValue(this, null);

                    if (value != null)
                        hashCode = (hashCode * RANDOM_PRIME_NUMBER) ^ value.GetHashCode();
                }

                if (domainSignatureProperties.Any())
                    return hashCode;

                // If no properties were flagged as being part of the domain signature of the object,
                // then simply return the hashcode of the base object as the hashcode.
                return base.GetHashCode();
            }
        }

        /// <summary>
        /// The method GetDomainObjectSignature should ONLY compare the properties which make up 
        /// the "domain signature" of the object.
        /// 
        /// If you choose NOT to override this method (which will be the most common scenario), 
        /// then you should decorate the appropriate property(s) with [DomainSignature] and they 
        /// will be compared automatically.  This is the preferred method.
        ///
        /// Alternatively, you may override this method to provide your own comparison routine.
        /// </summary>
        protected virtual bool HasSameDomainObjectSignatureAs(DomainSignatureComparable compareTo) {
            IEnumerable<PropertyInfo> domainSignatureProperties = GetDomainSignatureProperties();

            foreach (PropertyInfo property in domainSignatureProperties) {
                object valueOfThisObject = property.GetValue(this, null);
                object valueToCompareTo = property.GetValue(compareTo, null);

                if (valueOfThisObject == null && valueToCompareTo == null)
                    continue;

                if ((valueOfThisObject == null ^ valueToCompareTo == null) || 
                    (! valueOfThisObject.Equals(valueToCompareTo))) {
                    return false;
                }
            }

            // If we've gotten this far and domain signature properties were found, then we can
            // assume that everything matched; otherwise, if there were no domain signature 
            // properties, then simply return the default bahavior of Equals
            return domainSignatureProperties.Any() || base.Equals(compareTo);
        }

        private IEnumerable<PropertyInfo> GetDomainSignatureProperties() {
            return GetType().GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(DomainSignatureAttribute), true).Length > 0);
        }

        /// <summary>
        /// This particular magic number is often used in GetHashCode computations but is actually 
        /// quite random.  Resharper uses 397 as its number when overrideing GetHashCode, so it 
        /// either started there or has a deeper and more profound history than 42.
        /// </summary>
        private const int RANDOM_PRIME_NUMBER = 397;
    }
}
