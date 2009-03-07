using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections.Generic;
using SharpArch.Core.CommonValidator;

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
    /// Benefits include the addition of an Id property along with a consistent manner for comparing
    /// entities.
    /// 
    /// Since nearly all of the entities you create will have a type of int Id, this 
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
    public abstract class EntityWithTypedId<IdT> : ValidatableObject, IEntityWithTypedId<IdT>
    {
        #region IEntityWithTypedId Members

        /// <summary>
        /// Id may be of type string, int, custom type, etc.
        /// Setter is protected to allow unit tests to set this property via reflection and to allow 
        /// domain objects more flexibility in setting this for those objects with assigned Ids.
        /// It's virtual to allow NHibernate-backed objects to be lazily loaded.
        /// 
        /// This is ignored for XML serialization because it does not have a public setter (which is very much by design).
        /// See the FAQ within the documentation if you'd like to have the Id XML serialized.
        /// </summary>
        [XmlIgnore]
        [JsonProperty]
        public virtual IdT Id { get; protected set; }

        /// <summary>
        /// Transient objects are not associated with an item already in storage.  For instance,
        /// a Customer is transient if its Id is 0.  It's virtual to allow NHibernate-backed 
        /// objects to be lazily loaded.
        /// </summary>
        public virtual bool IsTransient() {
            return Id == null || Id.Equals(default(IdT));
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
            return GetType().GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(DomainSignatureAttribute), true));
        }

        public override bool Equals(object obj) {
            EntityWithTypedId<IdT> compareTo = obj as EntityWithTypedId<IdT>;

            if (ReferenceEquals(this, compareTo))
                return true;

            if (compareTo == null || !GetType().Equals(compareTo.GetTypeUnproxied()))
                return false;

            if (HasSameNonDefaultIdAs(compareTo))
                return true;

            // Since the Ids aren't the same, both of them must be transient to 
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
        /// Returns true if self and the provided entity have the same Id values 
        /// and the Ids are not of the default Id value
        /// </summary>
        private bool HasSameNonDefaultIdAs(EntityWithTypedId<IdT> compareTo) {
            return !IsTransient() &&
                  !compareTo.IsTransient() &&
                  Id.Equals(compareTo.Id);
        }

        #endregion
    }
}
