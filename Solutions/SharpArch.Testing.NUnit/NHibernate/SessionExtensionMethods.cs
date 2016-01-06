using NHibernate;

namespace SharpArch.Testing.NUnit.NHibernate
{
    public static class SessionExtensionMethods
    {
        public static void FlushAndEvict(this ISession session, object instance)
        {
            // Commits any changes up to this point to the database
            session.Flush();

            // Evicts the instance from the current session so that it can be loaded during testing;
            // this gives the test a clean slate, if you will, to work with
            session.Evict(instance);
        }
    }
}