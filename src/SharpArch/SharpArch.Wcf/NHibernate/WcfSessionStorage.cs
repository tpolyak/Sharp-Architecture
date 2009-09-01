using System.ServiceModel;
using SharpArch.Data.NHibernate;
using NHibernate;
using System;

namespace SharpArch.Wcf.NHibernate
{
	public class WcfSessionStorage : ISessionStorage
	{
		#region ISessionStorage Members

		public ISession GetSessionForKey(string factoryKey)
		{
			SessionInstanceExtension instance = GetSessionInstanceExtension();
			return instance.GetSessionForKey(factoryKey);
		}

		public void SetSessionForKey(string factoryKey, ISession session)
		{
			SessionInstanceExtension instance = GetSessionInstanceExtension();
			instance.SetSessionForKey(factoryKey, session);
		}

		public System.Collections.Generic.IEnumerable<ISession> GetAllSessions()
		{
			SessionInstanceExtension instance = GetSessionInstanceExtension();
			return instance.GetAllSessions();
		}

		#endregion

		private SessionInstanceExtension GetSessionInstanceExtension()
		{
			SessionInstanceExtension instance =
				OperationContext.Current.InstanceContext.Extensions.Find<SessionInstanceExtension>();

			if (instance == null)
				throw new Exception("SessionInstanceExtension not found in current OperationContext");

			return instance;
		}
	}
}
