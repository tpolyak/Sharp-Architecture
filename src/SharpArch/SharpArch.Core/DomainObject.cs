using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Validator.Engine;
using System.Xml.Serialization;

namespace SharpArch.Core
{
    /// <summary>
    /// Facilitates indicating which property(s) of a class describe the unique signature of a 
    /// business object.  See DomainObject.DomainSignatureProperties for how this is leveraged.
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
    public abstract class DomainObject : BaseObject, IValidatable
    {
        /// <summary>
        /// The property getter for SignatureProperties should ONLY compare the properties which make up 
        /// the "domain signature" of the object.
        /// 
        /// If you choose NOT to override this method (which will be the most common scenario), 
        /// then you should decorate the appropriate property(s) with [DomainSignature] and they 
        /// will be compared automatically.  This is the preferred method.
        ///
        /// Lazily loads the domain signature properties
        /// </summary>
        public override IEnumerable<PropertyInfo> SignatureProperties {
            get {
                return domainSignatureProperties ?? (domainSignatureProperties = GetType().GetProperties()
                        .Where(p => Attribute.IsDefined(p, typeof(DomainSignatureAttribute), true)).ToList());
            }
        }

        public virtual bool IsValid() {
            return Validator.IsValid(this);
        }

        /// <summary>
        /// If this property isn't ignored by the XML serializer, a "missing constructor" exception 
        /// is thrown during serialization.
        /// </summary>
        [XmlIgnoreAttribute]
        public virtual InvalidValue[] ValidationMessages {
            get {
                return Validator.Validate(this);
            }
        }

        /// <summary>
        /// Lazily loads the validation engine
        /// </summary>
        protected virtual ValidatorEngine Validator {
            get {
                if (validator == null) {
                    validator = new ValidatorEngine();
                }

                return validator;
            }
        }

        /// <summary>
        /// Talk to this property via the protected Validator property; that getter lazily 
        /// initializes this member.
        /// </summary>
        private ValidatorEngine validator;

        /// <summary>
        /// Talk to this property via the protected DomainSignatureProperties property; that getter lazily 
        /// initializes this member.
        /// </summary>
        private IEnumerable<PropertyInfo> domainSignatureProperties;
    }
}
