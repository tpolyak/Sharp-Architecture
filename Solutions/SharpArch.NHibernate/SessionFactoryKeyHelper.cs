namespace SharpArch.NHibernate
{
  using SharpArch.Domain;

  public static class SessionFactoryKeyHelper
  {
    public static string GetKey()
    {
      var provider = SafeServiceLocator<ISessionFactoryKeyProvider>.GetService();
      return provider.GetKey();
    }

    public static string GetKeyFrom(object anObject)
    {
      var provider = SafeServiceLocator<ISessionFactoryKeyProvider>.GetService();
      return provider.GetKeyFrom(anObject);
    }
  }
}