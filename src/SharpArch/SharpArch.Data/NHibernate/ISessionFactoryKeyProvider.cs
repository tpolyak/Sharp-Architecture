namespace SharpArch.Data.NHibernate
{
  public interface ISessionFactoryKeyProvider
  {
    /// <summary>
    /// Gets the session factory key.
    /// </summary>
    /// <param name="anObject">An optional object that may have an attribute used to determine the session factory key.</param>
    /// <returns></returns>
    string GetKey(object anObject);
  }
}