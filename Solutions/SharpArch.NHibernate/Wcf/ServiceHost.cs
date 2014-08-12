namespace SharpArch.NHibernate.Wcf
{
    using System;

    public class ServiceHost : System.ServiceModel.ServiceHost
    {
        public ServiceHost(Type serviceType, Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        protected override void OnOpening()
        {
            this.Description.Behaviors.Add(new ServiceBehavior());
            base.OnOpening();
        }
    }
}