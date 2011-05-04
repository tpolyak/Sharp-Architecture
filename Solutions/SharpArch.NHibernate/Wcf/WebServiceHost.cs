namespace SharpArch.NHibernate.Wcf
{
    using System;

    public class WebServiceHost : System.ServiceModel.Web.WebServiceHost
    {
        public WebServiceHost(Type serviceType, params Uri[] baseAddresses) : base(serviceType, baseAddresses)
        {
        }

        protected override void OnOpening()
        {
            Description.Behaviors.Add(new ServiceBehavior());
            base.OnOpening();
        }
    }
}
