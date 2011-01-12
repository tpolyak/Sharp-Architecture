using SharpArch.Core;

namespace SharpArch.Data.NHibernate
{
  public static class SessionFactoryKeyHelper
  {
    public static string GetKey(object anObject)
    {
      var provider = SafeServiceLocator<ISessionFactoryKeyProvider>.GetService();
      return provider.GetKey(anObject);
    }
  }
}