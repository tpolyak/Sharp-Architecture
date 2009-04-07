using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;
using SharpArch.Data.NHibernate;
using ISession = NHibernate.ISession;

namespace SharpArch.Wcf.NHibernate
{
    internal class DispatchMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext) {
            return NHibernateSession.Current;
        }

        public void BeforeSendReply(ref Message reply, object correlationState) {
            ISession session = correlationState as ISession;

            if (session == null)
                return;

            if (session.IsOpen)
                session.Close();

            if (NHibernateSession.Storage != null)
                NHibernateSession.Storage.Session = null;
        }
    }
}
