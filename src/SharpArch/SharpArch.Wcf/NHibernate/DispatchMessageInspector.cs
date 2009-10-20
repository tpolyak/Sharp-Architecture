using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;
using SharpArch.Data.NHibernate;
using ISession = NHibernate.ISession;

namespace SharpArch.Wcf.NHibernate
{
	internal class DispatchMessageInspector : IDispatchMessageInspector
	{
		public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
		{
			return null;
		}

		public void BeforeSendReply(ref Message reply, object correlationState)
		{
			NHibernateSession.CloseAllSessions();
		}
	}
}
