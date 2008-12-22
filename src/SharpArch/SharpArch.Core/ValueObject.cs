using System.Reflection;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Validator.Engine;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace SharpArch.Core
{
    /// <summary>
    /// Provides a standard base class for facilitating comparison of value objects using all the object's properties.
    /// 
    /// For a discussion of the implementation of Equals/GetHashCode, see 
    /// http://devlicio.us/blogs/billy_mccafferty/archive/2007/04/25/using-equals-gethashcode-effectively.aspx
    /// and http://groups.google.com/group/sharp-architecture/browse_thread/thread/f76d1678e68e3ece?hl=en for 
    /// an in depth and conclusive resolution.
    /// </summary>
    [Serializable]
    public abstract class ValueObject : BaseObject
    {
        /// <summary>
        /// The property getter for SignatureProperties for value objects should include the properties which make up 
        /// the entirety of the object's properties; that's part of the definition of a value object.
        /// </summary>
        protected override IEnumerable<PropertyInfo> SignatureProperties {
            get {
                if (valueSignatureProperties == null) {
                    valueSignatureProperties = GetType().GetProperties();
                }

                return valueSignatureProperties;
            }
        }

        /// <summary>
        /// Talk to this property via the protected DomainSignatureProperties property; that getter lazily 
        /// initializes this member.
        /// </summary>
        private IEnumerable<PropertyInfo> valueSignatureProperties;
    }
}
