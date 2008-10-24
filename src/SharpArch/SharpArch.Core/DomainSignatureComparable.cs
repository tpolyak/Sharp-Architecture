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
    /// For a discussion of this object, see 
    /// http://devlicio.us/blogs/billy_mccafferty/archive/2007/04/25/using-equals-gethashcode-effectively.aspx
    /// </summary>
    [Serializable]
    public abstract class DomainSignatureComparable
    {
        public override bool Equals(object obj) {
            DomainSignatureComparable compareTo = obj as DomainSignatureComparable;

            if (ReferenceEquals(this, compareTo))
                return true;

            return compareTo != null &&
                GetHashCode().Equals(compareTo.GetHashCode());
        }

        /// <summary>
        /// Used to compare two objects via <see cref="Equals"/>; although it's necessary for 
        /// NHibernate's use, this may also be useful for business logic purposes.
        /// </summary>
        public override int GetHashCode() {
            return GetDomainObjectSignature();
        }

        /// <summary>
        /// The method GetDomainObjectSignature should ONLY return the "business value 
        /// signature" of the object and not the ID, which is handled by <see cref="PersistentObject" />.
        /// 
        /// If you choose NOT to override this method, then you should decorate the appropriate 
        /// property(s) with [DomainSignature] and they will be compared automatically.  Using the 
        /// [DomainSignature] attribute is the preferred method.
        ///
        /// Alternatively, if you override this method, the general structure of the overridden method 
        /// should be as follows:  
        /// 
        /// return SomeProperty.GetHashCode() ^ SomeOtherProperty.GetHashCode();
        /// 
        /// </summary>
        protected virtual int GetDomainObjectSignature() {
            unchecked {
                // Since it's possible for two objects to return the same domain signature, 
                // even if they're of two different types, it's important to include the object's
                // type in the domain signature
                int typeHashCode = GetType().GetHashCode();

                int domainObjectSignature = typeHashCode;

                foreach (PropertyInfo property in GetDomainSignatureProperties()) {
                    object value = property.GetValue(this, null);

                    if (value != null) {
                        domainObjectSignature =
                            (domainObjectSignature * RANDOM_PRIME_NUMBER) ^
                            value.GetHashCode();
                    }
                }

                // If no properties were flagged as being part of the domain signature of the object,
                // then simply return the hashcode of the base object as the domain signature.  This
                // behaves as you would normally expect Equals to behave when comparing two objects.
                if (domainObjectSignature == typeHashCode)
                    return base.GetHashCode();

                return domainObjectSignature;
            }
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
