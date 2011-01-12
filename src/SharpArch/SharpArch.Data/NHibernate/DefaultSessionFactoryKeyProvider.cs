namespace SharpArch.Data.NHibernate
{
  /// <summary>
  /// Implementation of <see cref="ISessionFactoryKeyProvider" /> that uses 
  /// the <see cref="SessionFactoryAttribute" /> to determine the session
  /// factory key.
  /// </summary>
  public class DefaultSessionFactoryKeyProvider : ISessionFactoryKeyProvider
  {
    /// <summary>
    /// Gets the session factory key.
    /// </summary>
    /// <param name="anObject">An object that may have the <see cref="SessionFactoryAttribute"/> applied.</param>
    /// <returns></returns>
    public string GetKey(object anObject)
    {
      return SessionFactoryAttribute.GetKeyFrom(anObject);
    }
  }
}