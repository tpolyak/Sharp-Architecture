using NHibernate;
using NHibernate.Cfg;

namespace SharpArch.Data.NHibernate
{
	public interface ISessionStorage
	{
		ISession Session { get; set; }
	}

	public class SimpleSessionStorage : ISessionStorage
	{
	    public ISession Session { get; set; }
	}

	public static class NHibernateSession
	{
		public static void Init(ISessionStorage storage, string cfgFile) {
			Configuration cfg = new Configuration();

            if (cfgFile == null)
				cfg = cfg.Configure();
			else
				cfg = cfg.Configure(cfgFile);

			SessionFactory = cfg.BuildSessionFactory();
			Storage = storage;
		}

		public static ISessionFactory SessionFactory { get; set; }
		public static ISessionStorage Storage { get; set; }

		public static ISession Current {
			get {
				ISession session = Storage.Session;
				
                if (session == null) {
					session = SessionFactory.OpenSession();
					Storage.Session = session;
				}

                return session;
			}
		}
	}
}
