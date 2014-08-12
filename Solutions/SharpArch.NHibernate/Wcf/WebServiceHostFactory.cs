namespace SharpArch.NHibernate.Wcf
{
    using System;

    public class WebServiceHostFactory : System.ServiceModel.Activation.WebServiceHostFactory
    {
        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new WebServiceHost(serviceType, baseAddresses);
        }
    }
}
