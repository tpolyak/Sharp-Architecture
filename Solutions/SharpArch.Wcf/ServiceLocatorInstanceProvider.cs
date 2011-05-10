namespace SharpArch.Wcf
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    using Microsoft.Practices.ServiceLocation;

    public class ServiceLocatorInstanceProvider : IInstanceProvider
    {
        private readonly Type serviceType;

        public ServiceLocatorInstanceProvider(Type serviceType)
        {
            this.serviceType = serviceType;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return this.GetInstance(instanceContext, null);
        }

        /// <summary>
        ///     Replicates the behavior of <see cref = "SharpArch.Domain.SafeServiceLocator{T}" /> 
        ///     to create an instance of the requested WCF service.
        /// </summary>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            object service;

            try
            {
                service = ServiceLocator.Current.GetService(this.serviceType);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException(
                    "ServiceLocator has not been initialized; " + "I was trying to retrieve " + this.serviceType);
            }
            catch (ActivationException)
            {
                throw new ActivationException(
                    "The needed dependency of type " + this.serviceType.Name +
                    " could not be located with the ServiceLocator. You'll need to register it with " +
                    "the Common Service Locator (CSL) via your IoC's CSL adapter.");
            }

            return service;
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
    }
}