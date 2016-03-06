namespace SharpArch.NHibernate
{
    using JetBrains.Annotations;

    /// <summary>
    /// Implementation of <see cref="ISessionFactoryKeyProvider" /> that uses 
    /// the <see cref="SessionFactoryAttribute" /> to determine the session
    /// factory key.
    /// </summary>
    [PublicAPI]
    public class DefaultSessionFactoryKeyProvider : ISessionFactoryKeyProvider
  {
        /// <summary>
        /// Gets the session factory key.
        /// </summary>
        public string GetKey()
    {
        return NHibernateSessionFactoryBuilder.DefaultConfigurationName;
    }

    /// <summary>
    /// Gets the session factory key.
    /// </summary>
    /// <param name="anObject">An object that may have the <see cref="SessionFactoryAttribute"/> applied.</param>
    /// <returns></returns>
    public string GetKeyFrom(object anObject)
    {
      return SessionFactoryAttribute.GetKeyFrom(anObject);
    }
  }
}