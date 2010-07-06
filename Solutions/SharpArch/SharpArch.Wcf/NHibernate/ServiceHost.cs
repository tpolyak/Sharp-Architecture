using System.ServiceModel;
using System;

namespace SharpArch.Wcf.NHibernate
{
    public class ServiceHost : System.ServiceModel.ServiceHost
    {
        public ServiceHost(Type serviceType, Uri[] baseAddresses)
            : base(serviceType, baseAddresses) {
        }

        protected override void OnOpening() {
            Description.Behaviors.Add(new ServiceBehavior());
            base.OnOpening();
        }
    }
}
