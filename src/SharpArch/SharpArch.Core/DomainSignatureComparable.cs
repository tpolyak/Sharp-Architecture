using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SharpArch.Core
{
    /// <summary>
    /// Facilitates indicating which property(s) of a class describe the unique signature of a 
    /// business object.  See PersistentObject.GetDomainObjectSignature for how this is leveraged.
    /// </summary>
    public class DomainSignatureAttribute : Attribute { }

    /// <summary>
    /// Provides a standard base class for facilitating comparison of domain objects via the 
    /// <see cref="DomainSignatureAttribute" />.
    /// 
    /// For a discussion of this object, see 
    /// http://devlicio.us/blogs/billy_mccafferty/archive/2007/04/25/using-equals-gethashcode-effectively.aspx
    /// </summary>
    public abstract class DomainSignatureComparable
    {
        public override bool Equals(object obj) {
            DomainSignatureComparable compareTo = obj as DomainSignatureComparable;

            return compareTo != null &&
                GetHashCode().Equals(compareTo.GetHashCode());
        }

        /// <summary>
        /// Used to compare two objects via <see cref="Equals"/>; although it's necessary for 
        /// NHibernate's use, this may also be useful for business logic purposes.
        /// </summary>
        public override int GetHashCode() {
            // Since it's possible for two objects to return the same domain signature, 
            // even if they're of two different types, it's important to include the object's
            // type in the domain signature
            return GetType().GetHashCode() ^ GetDomainObjectSignature();
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
            int domainObjectSignature = 0;

            foreach (PropertyInfo property in GetType().GetProperties()) {
                if (IsPartOfDomainSignature(property)) {
                    object value = property.GetValue(this, null);

                    if (value != null) {
                        domainObjectSignature = domainObjectSignature ^ value.GetHashCode();
                    }
                }
            }

            // If no properties were flagged as being part of the domain signature of the object,
            // then simply return the hashcode of the base object as the domain signature.  This
            // behaves as you would normally expect Equals to behave when comparing two objects.
            if (domainObjectSignature == 0) {
                return base.GetHashCode();
            }

            return domainObjectSignature;
        }

        private bool IsPartOfDomainSignature(PropertyInfo property) {
            return property.GetCustomAttributes(typeof(DomainSignatureAttribute), true).Length > 0;
        }
    }
}
