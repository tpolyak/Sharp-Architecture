using System.ServiceModel;
using SharpArch.Data.NHibernate;
using NHibernate;
using System;

namespace SharpArch.Wcf.NHibernate
{
    internal class SessionInstanceExtension : IExtension<InstanceContext>, ISessionStorage
    {
        public SessionInstanceExtension() { }

        public void Attach(InstanceContext owner) { }
        public void Detach(InstanceContext owner) { }

		#region ISessionStorage Members

		public ISession GetSessionForKey(string factoryKey)
		{
			return storage.GetSessionForKey(factoryKey);
		}

		public void SetSessionForKey(string factoryKey, ISession session)
		{
			storage.SetSessionForKey(factoryKey, session);
		}

		public System.Collections.Generic.IEnumerable<ISession> GetAllSessions()
		{
			return storage.GetAllSessions();
		}

		#endregion

		private SimpleSessionStorage storage = new SimpleSessionStorage();
	}
}
