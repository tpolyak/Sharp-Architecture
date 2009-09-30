using System;
using System.Web;
using NHibernate;
using SharpArch.Data;
using SharpArch.Data.NHibernate;

namespace SharpArch.Web.NHibernate
{
	public class WebSessionStorage : ISessionStorage
	{
		public WebSessionStorage(HttpApplication app)
		{
			app.EndRequest += Application_EndRequest;
		}

		public ISession GetSessionForKey(string factoryKey)
		{
			SimpleSessionStorage storage = GetSimpleSessionStorage();
			return storage.GetSessionForKey(factoryKey);
		}

		public void SetSessionForKey(string factoryKey, ISession session)
		{
			SimpleSessionStorage storage = GetSimpleSessionStorage();
			storage.SetSessionForKey(factoryKey, session);
		}

		public System.Collections.Generic.IEnumerable<ISession> GetAllSessions()
		{
			SimpleSessionStorage storage = GetSimpleSessionStorage();
			return storage.GetAllSessions();
		}

		private SimpleSessionStorage GetSimpleSessionStorage()
		{
			HttpContext context = HttpContext.Current;
			SimpleSessionStorage storage = context.Items[HttpContextSessionStorageKey] as SimpleSessionStorage;
			if (storage == null)
			{
				storage = new SimpleSessionStorage();
				context.Items[HttpContextSessionStorageKey] = storage;
			}
			return storage;
		}

		private static readonly string HttpContextSessionStorageKey = "HttpContextSessionStorageKey";

		private void Application_EndRequest(object sender, EventArgs e)
		{
			NHibernateSession.CloseAllSessions();

			HttpContext context = HttpContext.Current;
			context.Items.Remove(HttpContextSessionStorageKey);
		}
	}
}
