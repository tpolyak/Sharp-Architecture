namespace SharpArch.NHibernate
{
  /// <summary>
  /// Implementation of <see cref="ISessionFactoryKeyProvider" /> that uses 
  /// the <see cref="SessionFactoryAttribute" /> to determine the session
  /// factory key.
  /// </summary>
  public class DefaultSessionFactoryKeyProvider : ISessionFactoryKeyProvider
  {
    public string GetKey()
    {
      return NHibernateSession.DefaultFactoryKey;
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