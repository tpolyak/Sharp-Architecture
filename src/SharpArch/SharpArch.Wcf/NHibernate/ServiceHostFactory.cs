using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpArch.Wcf.NHibernate
{
    public class ServiceHostFactory : System.ServiceModel.Activation.ServiceHostFactory
    {
        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses) {
            return new ServiceHost(serviceType, baseAddresses);
        }
    }
}
