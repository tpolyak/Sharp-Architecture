namespace SharpArch.NHibernate
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    ///     Provides the ability to decorate repositories with an attribute defining the factory key
    ///     for the given repository; accordingly, the respective session factory will be used to 
    ///     communicate with the database.  This allows you to declare different repositories to 
    ///     communicate with different databases.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [PublicAPI]
    public sealed class SessionFactoryAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionFactoryAttribute"/> class.
        /// </summary>
        /// <param name="factoryKey">The factory key.</param>
        public SessionFactoryAttribute(string factoryKey)
        {
            this.FactoryKey = factoryKey;
        }

        /// <summary>
        /// Session factory key.
        /// </summary>
        public string FactoryKey { get; }

        /// <summary>
        ///     Global method to retrieve the factory key from an object, as defined in its 
        ///     SessionFactoryAttribute, if available.  Defaults to the DefaultFactoryKey 
        ///     if not found.
        /// </summary>
        [NotNull]
        public static string GetKeyFrom([NotNull] object target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            // todo: cache sessionKey value
            
            var objectType = target.GetType();

            var attributes = objectType.GetCustomAttributes(typeof(SessionFactoryAttribute), true);

            if (attributes.Length > 0)
            {
                var attribute = (SessionFactoryAttribute)attributes[0];
                return attribute.FactoryKey;
            }

            return NHibernateSessionFactoryBuilder.DefaultConfigurationName;
        }
    }
}