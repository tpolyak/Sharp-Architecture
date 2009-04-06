using System.Collections.Generic;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace SharpArch.Wcf.NHibernate
{
    internal class InstanceCreationInitializer : IInstanceContextInitializer
    {
        public void Initialize(InstanceContext instanceContext, Message message) {
            instanceContext.Extensions.Add(new SessionInstanceExtension());
        }
    }
}
