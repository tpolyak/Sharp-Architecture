using System;
using Castle.MicroKernel.ModelBuilder;
using Castle.MicroKernel;
using Castle.Core;
using Castle.Core.Configuration;
using System.ServiceModel;

namespace SharpArch.WcfClient.Castle
{
    /// <summary>
    /// This facility may be registered within your web application to automatically look for and close
    /// WCF connections.  This eliminates all the redundant code for closing the connection and aborting
    /// if any appropriate exceptions are encountered.  See documentation for setting up and using this
    /// Castle facility.
    /// </summary>
    public class WcfSessionFacility : IFacility
    {
        public const string ManageWcfSessionsKey = "ManageWcfSessions";

        public void Init(IKernel kernel, IConfiguration facilityConfig) {
            kernel.ComponentDestroyed += new ComponentInstanceDelegate(kernel_ComponentDestroyed);
        }

        void kernel_ComponentDestroyed(ComponentModel model, object instance) {
            bool shouldManage = false;
            object value = model.ExtendedProperties[ManageWcfSessionsKey];

            if (value != null) {
                shouldManage = (bool)value;
            }
            
            if (!shouldManage)
                return;

            ICommunicationObject communicationObject = instance as ICommunicationObject;

            if (communicationObject != null) {
                try {
                    if (communicationObject.State != CommunicationState.Faulted) {
                        communicationObject.Close();
                    }
                    else {
                        communicationObject.Abort();
                    }
                }
                catch (CommunicationException) {
                    communicationObject.Abort();
                }
                catch (System.TimeoutException) {
                    communicationObject.Abort();
                }
                catch (Exception) {
                    communicationObject.Abort();
                    throw;
                }
            }
        }

        public void Terminate() { }
    }
}