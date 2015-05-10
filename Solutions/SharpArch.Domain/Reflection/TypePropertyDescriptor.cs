namespace SharpArch.Domain.Reflection
{
    using System;
    using System.Reflection;

    /// <summary>
    ///     Contains injectable properties per type.
    /// </summary>
    [Serializable]
    public class TypePropertyDescriptor : IEquatable<TypePropertyDescriptor>
    {
        private static readonly PropertyInfo[] EmptyArray = new PropertyInfo[0];

        /// <summary>
        ///     Initializes a new instance of the <see cref="TypePropertyDescriptor" /> class.
        /// </summary>
        /// <param name="ownerType">Type of the object.</param>
        /// <param name="properties">The injectable properties.</param>
        public TypePropertyDescriptor(Type ownerType, PropertyInfo[] properties)
        {
            if (ownerType == null) throw new ArgumentNullException("ownerType");

            OwnerType = ownerType;
            if (properties != null && properties.Length > 0)
                Properties = properties;
            else
                Properties = EmptyArray;
        }

        /// <summary>
        ///     Owner type. 
        /// </summary>
        public Type OwnerType { get; private set; }

        /// <summary>
        ///     Gets the injectable properties.
        /// </summary>
        /// <value>
        ///     The injectable properties.
        /// </value>
        public PropertyInfo[] Properties { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether <see cref="OwnerType" /> has injectable properties.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has injectable properties; otherwise, <c>false</c>.
        /// </value>
        public bool HasProperties()
        {
            return Properties.Length > 0;
        }

        public bool Equals(TypePropertyDescriptor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return OwnerType.Equals(other.OwnerType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as TypePropertyDescriptor;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            return OwnerType.GetHashCode();
        }

        public override string ToString()
        {
            return string.Concat("Type: ", OwnerType.AssemblyQualifiedName, "Properties: ", Properties.Length);
        }
    }
}