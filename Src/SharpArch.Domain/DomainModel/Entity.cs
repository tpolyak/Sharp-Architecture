namespace SharpArch.Domain.DomainModel
{
#if !NULLABLE_REFERENCE_TYPES
    #pragma warning disable 8618
    #pragma warning disable 8604
#endif
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;
    using JetBrains.Annotations;

    /// <summary>
    ///     For a discussion of this object, see
    ///     http://devlicio.us/blogs/billy_mccafferty/archive/2007/04/25/using-equals-gethashcode-effectively.aspx
    /// </summary>
    [Serializable]
    [PublicAPI]
    public abstract class Entity<TId> : ValidatableObject, IEntity<TId>, IEntity,
        IEquatable<Entity<TId>>
        where TId : IEquatable<TId>
    {
        /// <summary>
        ///     To help ensure hash code uniqueness, a carefully selected random number multiplier
        ///     is used within the calculation.  Goodrich and Tamassia's Data Structures and
        ///     Algorithms in Java asserts that 31, 33, 37, 39 and 41 will produce the fewest number
        ///     of collisions.  See http://computinglife.wordpress.com/2008/11/20/why-do-hash-functions-use-prime-numbers/
        ///     for more information.
        /// </summary>
        const int HashMultiplier = 31;

        int? _cachedHashcode;

        /// <summary>
        ///     Gets or sets the ID.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The ID may be of type <c>string</c>, <c>int</c>, a custom type, etc.
        ///         The setter is protected to allow unit tests to set this property via reflection
        ///         and to allow domain objects more flexibility in setting this for those objects
        ///         with assigned IDs. It's virtual to allow NHibernate-backed objects to be lazily
        ///         loaded. This is ignored for XML serialization because it does not have a public
        ///         setter (which is very much by design). See the FAQ within the documentation if
        ///         you'd like to have the ID XML serialized.
        ///     </para>
        /// </remarks>
        [XmlIgnore]
#if NULLABLE_REFERENCE_TYPES
        [MaybeNull]
#endif
        public virtual TId Id { get; protected set; }


        /// <inheritdoc />
        public virtual object? GetId()
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (Id is null) return null;
            return Id.Equals(default!)
                // ReSharper disable once RedundantCast
                ? (object?) null
                : Id;
        }

        /// <summary>
        ///     Returns a value indicating whether the current object is transient.
        /// </summary>
        /// <remarks>
        ///     Transient objects are not associated with an item already in storage. For instance,
        ///     a Customer is transient if its ID is 0.  It's virtual to allow NHibernate-backed
        ///     objects to be lazily loaded.
        /// </remarks>
        public virtual bool IsTransient()
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            return Id is null
                || Id.Equals(default!);
        }

        /// <inheritdoc />
        public virtual bool Equals(Entity<TId>? other)
        {
            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (other == null || GetType() != other.GetTypeUnproxied()) {
                return false;
            }

            if (HasSameNonDefaultIdAs(other)) {
                return true;
            }

            // Since the Ids aren't the same, both of them must be transient to 
            // compare domain signatures; because if one is transient and the 
            // other is a persisted entity, then they cannot be the same object.
            return IsTransient() && other.IsTransient() && HasSameObjectSignatureAs(other);
            
        }

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current <see cref="object" />.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            var compareTo = obj as Entity<TId>;
            return Equals(compareTo);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        /// <remarks>
        ///     This is used to provide the hash code identifier of an object using the signature
        ///     properties of the object; although it's necessary for NHibernate's use, this can
        ///     also be useful for business logic purposes and has been included in this base
        ///     class, accordingly. Since it is recommended that GetHashCode change infrequently,
        ///     if at all, in an object's lifetime, it's important that properties are carefully
        ///     selected which truly represent the signature of an object.
        /// </remarks>
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode", Justification = "By design")]
        public override int GetHashCode()
        {
            if (_cachedHashcode.HasValue) {
                return _cachedHashcode.Value;
            }

            if (IsTransient()) {
                _cachedHashcode = base.GetHashCode();
            }
            else {
                unchecked
                {
                    // It's possible for two objects to return the same hash code based on 
                    // identically valued properties, even if they're of two different types, 
                    // so we include the object's type in the hash calculation
                    int hashCode = GetType().GetHashCode() * HashMultiplier;

                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    if (Id is null)
                    {
                        _cachedHashcode = hashCode;
                    }
                    else
                    {
                        _cachedHashcode = hashCode ^ Id.GetHashCode();
                    }
                }
            }

            return _cachedHashcode.Value;
        }

        /// <summary>
        ///     Returns the signature properties that are specific to the type of the current object.
        /// </summary>
        /// <remarks>
        ///     If you choose NOT to override this method (which will be the most common scenario),
        ///     then you should decorate the appropriate property(s) with the <see cref="DomainSignatureAttribute" />
        ///     attribute and they will be compared automatically. This is the preferred method of
        ///     managing the domain signature of entity objects. This ensures that the entity has at
        ///     least one property decorated with the <see cref="DomainSignatureAttribute" /> attribute.
        /// </remarks>
        protected override PropertyInfo[] GetTypeSpecificSignatureProperties()
        {
            return
                GetType().GetProperties().Where(p => p.IsDefined(typeof (DomainSignatureAttribute), true)).ToArray();

        }

        /// <summary>
        ///     Returns a value indicating whether the current entity and the provided entity have
        ///     the same ID values and the IDs are not of the default ID value.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the current entity and the provided entity have the same ID values and the IDs are not of the
        ///     default ID value; otherwise; <c>false</c>.
        /// </returns>
        bool HasSameNonDefaultIdAs(Entity<TId> compareTo)
        {
#if NULLABLE_REFERENCE_TYPES
            return !IsTransient() && !compareTo.IsTransient() && Id!.Equals(compareTo.Id!);
#else
            return !IsTransient() && !compareTo.IsTransient() && Id.Equals(compareTo.Id);
#endif
        }

        /// <summary>
        ///     Check whether entities are equal.
        /// </summary>
        /// <param name="left">Entity to compare.</param>
        /// <param name="right">Entity to compare.</param>
        /// <returns><c>true</c> if entities are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
            => Equals(left, right);

        /// <summary>
        ///     Check whether entities are not equal.
        /// </summary>
        /// <param name="left">Entity to compare.</param>
        /// <param name="right">Entity to compare.</param>
        /// <returns><c>true</c> if entities are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
            => !Equals(left, right);
    }

#if !NULLABLE_REFERENCE_TYPES
    #pragma warning restore 8618
    #pragma warning restore 8604
#endif

}
