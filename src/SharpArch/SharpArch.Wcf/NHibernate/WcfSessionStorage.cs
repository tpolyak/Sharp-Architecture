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
            if (OperationContext.Current == null)
            {
                return FallbackSessionStorage.GetSessionForKey(factoryKey);
            }

            SessionInstanceExtension instance = GetSessionInstanceExtension();
            return instance.GetSessionForKey(factoryKey);
        }

        public void SetSessionForKey(string factoryKey, ISession session)
        {
            if (OperationContext.Current == null)
            {
                FallbackSessionStorage.SetSessionForKey(factoryKey, session);
                return;
            }

            SessionInstanceExtension instance = GetSessionInstanceExtension();
            instance.SetSessionForKey(factoryKey, session);
        }

        public System.Collections.Generic.IEnumerable<ISession> GetAllSessions()
        {
            if (OperationContext.Current == null)
            {
                return FallbackSessionStorage.GetAllSessions();
            }

            SessionInstanceExtension instance = GetSessionInstanceExtension();
            return instance.GetAllSessions();
        }

        #endregion

        private ISessionStorage fallbackSessionStorage;

        private ISessionStorage FallbackSessionStorage
        {
            get
            {
                return fallbackSessionStorage ?? (fallbackSessionStorage = new SimpleSessionStorage());
            }

            set
            {
                fallbackSessionStorage = value;
            }
        }

        public WcfSessionStorage()
            : this(null)
        {
        }

        public WcfSessionStorage(ISessionStorage fallbackSessionStorage)
        {
            this.FallbackSessionStorage = fallbackSessionStorage;
        }

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
