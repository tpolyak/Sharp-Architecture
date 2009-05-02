using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpArch.Data.NHibernate
{
    /// <summary>
    /// Provides the ability to decorate repositories with an attribute defining the factory key
    /// for the given repository; accordingly, the respective session factory will be used to 
    /// communicate with the database.  This allows you to declare different repositories to 
    /// communicate with different databases.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SessionFactoryAttribute : Attribute
    {
        public readonly string FactoryKey;

        public SessionFactoryAttribute(string factoryKey) {
            FactoryKey = factoryKey;
        }

        /// <summary>
        /// Global method to retrieve the factory key from an object, as defined in its 
        /// SessionFactoryAttribute, if available.  Defaults to the DefaultFactoryKey 
        /// if not found.
        /// </summary>
        public static string GetKeyFrom(object anObject) {
            if (!NHibernateSession.IsConfiguredForMultipleDatabases())
                return NHibernateSession.DefaultFactoryKey;

            Type objectType = anObject.GetType();

            object[] attributes = objectType.GetCustomAttributes(typeof(SessionFactoryAttribute), true);

            if (attributes.Length > 0) {
                SessionFactoryAttribute attribute = (SessionFactoryAttribute)attributes[0];
                return attribute.FactoryKey;
            }

            return NHibernateSession.DefaultFactoryKey;
        }
    }
}
