namespace SharpArch.Wcf.NHibernate
{
    using System.Collections.Generic;
    using System.ServiceModel;

    using global::NHibernate;

    using SharpArch.Infrastructure.NHibernate;

    internal class SessionInstanceExtension : IExtension<InstanceContext>, ISessionStorage
    {
        private readonly SimpleSessionStorage storage = new SimpleSessionStorage();

        public void Attach(InstanceContext owner)
        {
        }

        public void Detach(InstanceContext owner)
        {
        }

        public IEnumerable<ISession> GetAllSessions()
        {
            return this.storage.GetAllSessions();
        }

        public ISession GetSessionForKey(string factoryKey)
        {
            return this.storage.GetSessionForKey(factoryKey);
        }

        public void SetSessionForKey(string factoryKey, ISession session)
        {
            this.storage.SetSessionForKey(factoryKey, session);
        }
    }
}