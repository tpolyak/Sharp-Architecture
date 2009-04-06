using System.ServiceModel;
using SharpArch.Data.NHibernate;
using NHibernate;

namespace SharpArch.Wcf.NHibernate
{
    internal class SessionInstanceExtension : IExtension<InstanceContext>, ISessionStorage
    {
        public SessionInstanceExtension() { }

        public void Attach(InstanceContext owner) { }
        public void Detach(InstanceContext owner) { }

        public ISession Session { get; set; }
    }
}
