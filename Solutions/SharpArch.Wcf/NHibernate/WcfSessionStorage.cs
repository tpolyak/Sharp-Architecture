namespace SharpArch.Wcf.NHibernate
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;

    using global::NHibernate;

    using SharpArch.Infrastructure.NHibernate;

    public class WcfSessionStorage : ISessionStorage
    {
        public IEnumerable<ISession> GetAllSessions()
        {
            var instance = GetSessionInstanceExtension();
            return instance.GetAllSessions();
        }

        public ISession GetSessionForKey(string factoryKey)
        {
            var instance = GetSessionInstanceExtension();
            return instance.GetSessionForKey(factoryKey);
        }

        public void SetSessionForKey(string factoryKey, ISession session)
        {
            var instance = GetSessionInstanceExtension();
            instance.SetSessionForKey(factoryKey, session);
        }

        private static SessionInstanceExtension GetSessionInstanceExtension()
        {
            var instance = OperationContext.Current.InstanceContext.Extensions.Find<SessionInstanceExtension>();

            if (instance == null)
            {
                throw new Exception("SessionInstanceExtension not found in current OperationContext");
            }

            return instance;
        }
    }
}