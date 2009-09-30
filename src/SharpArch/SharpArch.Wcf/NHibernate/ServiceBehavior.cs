using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;

namespace SharpArch.Wcf.NHibernate
{
    internal class ServiceBehavior : IServiceBehavior
    {
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) {
            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers) {
                ChannelDispatcher channelDispatcher = channelDispatcherBase as ChannelDispatcher;
                if (channelDispatcher != null) {
                    foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints) {
                        endpointDispatcher.DispatchRuntime.InstanceProvider = new ServiceLocatorInstanceProvider(serviceDescription.ServiceType);
                        endpointDispatcher.DispatchRuntime.InstanceContextInitializers.Add(new InstanceCreationInitializer());
                        endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new DispatchMessageInspector());
                    }
                }
            }
        }

        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters) {
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }
    }
}
