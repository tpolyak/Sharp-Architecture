using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System;
using System.ServiceModel.Channels;
using SharpArch.Core;
using Microsoft.Practices.ServiceLocation;

namespace SharpArch.Wcf
{
    public class ServiceLocatorInstanceProvider : IInstanceProvider
    {
        public ServiceLocatorInstanceProvider(Type serviceType) {
            this.serviceType = serviceType;
        }

        public object GetInstance(InstanceContext instanceContext) {
            return GetInstance(instanceContext, null);
        }

        /// <summary>
        /// Replicates the behavior of <see cref="SharpArch.Core.SafeServiceLocator" /> 
        /// to create an instance of the requested WCF service.
        /// </summary>
        public object GetInstance(InstanceContext instanceContext, Message message) {
            object service;

            try {
                service = ServiceLocator.Current.GetService(serviceType);
            }
            catch (NullReferenceException) {
                throw new NullReferenceException("ServiceLocator has not been initialized; " +
                    "I was trying to retrieve " + serviceType.ToString());
            }
            catch (ActivationException) {
                throw new ActivationException("The needed dependency of type " + serviceType.Name +
                    " could not be located with the ServiceLocator. You'll need to register it with " +
                    "the Common Service Locator (CSL) via your IoC's CSL adapter.");
            }

            return service;
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance) { }

        private readonly Type serviceType;
    }
}
