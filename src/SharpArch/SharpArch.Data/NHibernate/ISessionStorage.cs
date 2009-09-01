using NHibernate;
using System.Collections.Generic;

namespace SharpArch.Data.NHibernate
{
    public interface ISessionStorage
    {
		ISession GetSessionForKey(string factoryKey);
		void SetSessionForKey(string factoryKey, ISession session);
		IEnumerable<ISession> GetAllSessions();
    }
}
