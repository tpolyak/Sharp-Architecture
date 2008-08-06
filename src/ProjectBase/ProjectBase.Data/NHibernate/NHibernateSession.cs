using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Web;
using NHibernate.Cfg;
using System.Configuration;

namespace ProjectBase.Data.NHibernate
{
	public interface ISessionStorage
	{
		ISession Session { get; set; }
	}

	public class SimpleSessionStorage : ISessionStorage
	{
		ISession session;

		#region ISessionStorage Members

		public ISession Session {
			get { return this.session; }
			set { this.session = value; }
		}

		#endregion
	}

	public static class NHibernateSession
	{
		public static void Init(ISessionStorage storage, string cfgFile) {
			Configuration cfg = new Configuration();

            if (cfgFile == null)
				cfg = cfg.Configure();
			else
				cfg = cfg.Configure(cfgFile);

			NHibernateSession.SessionFactory = cfg.BuildSessionFactory();
			NHibernateSession.Storage = storage;
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
