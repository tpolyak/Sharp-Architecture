using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections.Generic;
using NHibernate.Validator.Engine;
using SharpArch.Core.DomainModel;

namespace SharpArch.Core.DomainModel
{
    /// <summary>
    /// Facilitates indicating which property(s) describe the unique signature of an 
    /// entity.  See Entity.GetTypeSpecificSignatureProperties() for when this is leveraged.
    /// </summary>
    /// <remarks>
    /// This is intended for use with <see cref="Entity" />.  It may NOT be used on a <see cref="ValueObject"/>.
    /// </remarks>
    [Serializable]
    public class DomainSignatureAttribute : Attribute { }

    /// <summary>
    /// Provides a base class for your objects which will be persisted to the database.
    /// Benefits include the addition of an ID property along with a consistent manner for comparing
    /// entities.
    /// 
    /// Since nearly all of the entities you create will have a type of int ID, this 
    /// base class leverages this assumption.  If you want an entity with a type other 
    /// than int, such as string, then use <see cref="EntityWithTypedId{IdT}" /> instead.
    /// </summary>
    [Serializable]
    public abstract class Entity : EntityWithTypedId<int> { }

    /// <summary>
    /// For a discussion of this object, see 
    /// http://devlicio.us/blogs/billy_mccafferty/archive/2007/04/25/using-equals-gethashcode-effectively.aspx
    /// </summary>
    [Serializable]
    public abstract class EntityWithTypedId<IdT> : BaseObject, IEntityWithTypedId<IdT>, IValidatable
    {
        #region IValidatable Members

        public virtual bool IsValid() {
            return Validator.IsValid(this);
        }

        /// <summary>
        /// If this property isn't ignored by the XML serializer, a "missing constructor" exception 
        /// is thrown during serialization.
        /// </summary>
        public virtual InvalidValue[] GetValidationMessages() {
            return Validator.Validate(this);
        }

        private ValidatorEngine Validator {
            get {
                if (validator == null) {
                    validator = new ValidatorEngine();
                }

                return validator;
            }
        }

        [ThreadStatic]
        private static ValidatorEngine validator;

        #endregion

        #region IEntityWithTypedId Members

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

        #endregion

        #region Entity comparison support

        /// <summary>
        /// The property getter for SignatureProperties should ONLY compare the properties which make up 
        /// the "domain signature" of the object.
        /// 
        /// If you choose NOT to override this method (which will be the most common scenario), 
        /// then you should decorate the appropriate property(s) with [DomainSignature] and they 
        /// will be compared automatically.  This is the preferred method of managing the domain
        /// signature of entity objects.
        /// </summary>
        /// <remarks>
        /// This ensures that the entity has at least one property decorated with the 
        /// [DomainSignature] attribute.
        /// </remarks>
        protected override IEnumerable<PropertyInfo> GetTypeSpecificSignatureProperties() {
            IEnumerable<PropertyInfo> signatureProperties = GetType().GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(DomainSignatureAttribute), true));

            Check.Ensure(signatureProperties.Any(), 
                "No properties were found within " + GetType().ToString() + " having the " +
                "[DomainSignature] attribute. An entity object without a domain signature is more " + 
                "akin to a \"value object.\" For further assistance, please see the documentation for " +
                "a thorough discussion of what defines the domain signature of an entity object. " +
                "As a quick example, suppose you have an employee object with a social security " +
                "number property; you would likely put the [DomainSignature] attribute above this " +
                "property. Note that objects frequently have domain signature made up of multiple " +
                "properties. You can put the [DomainSignature] attribute above each property that " +
                "makes of the domain signature.  Alternatively, you can inherit from ValueObject " +
                "if that fits your needs better.");

            return signatureProperties;
        }

        public override bool Equals(object obj) {
            EntityWithTypedId<IdT> compareTo = obj as EntityWithTypedId<IdT>;

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
        /// Returns true if self and the provided entity have the same ID values 
        /// and the IDs are not of the default ID value
        /// </summary>
        private bool HasSameNonDefaultIdAs(EntityWithTypedId<IdT> compareTo) {
            return !IsTransient() &&
                  !compareTo.IsTransient() &&
                  ID.Equals(compareTo.ID);
        }

        #endregion
    }
}
