using NHibernate;
using System.Collections.Generic;

namespace SharpArch.Data.NHibernate
{
    public class SimpleSessionStorage : ISessionStorage
    {
		private Dictionary<string, ISession> storage = new Dictionary<string, ISession>();

		public SimpleSessionStorage() { }

		/// <summary>
		/// Returns the session associated with the specified factoryKey or
		/// null if the specified factoryKey is not found.
		/// </summary>
		/// <param name="factoryKey"></param>
		/// <returns></returns>
		public ISession GetSessionForKey(string factoryKey)
		{
			ISession session;
			if (!this.storage.TryGetValue(factoryKey, out session))
				return null;
			return session;
		}

		/// <summary>
		/// Stores the session into a dictionary using the specified factoryKey.
		/// If a session already exists by the specified factoryKey, 
		/// it gets overwritten by the new session passed in.
		/// </summary>
		/// <param name="factoryKey"></param>
		/// <param name="session"></param>
		public void SetSessionForKey(string factoryKey, ISession session)
		{
			this.storage[factoryKey] = session;
		}

		/// <summary>
		/// Returns all the values of the internal dictionary of sessions.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ISession> GetAllSessions()
		{
			return this.storage.Values;
		}
	}
}
