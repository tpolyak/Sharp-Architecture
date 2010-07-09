namespace SharpArch.WcfClient.Castle
{
    using System;
    using System.ServiceModel;

    using global::Castle.Core;
    using global::Castle.Core.Configuration;
    using global::Castle.MicroKernel;

    /// <summary>
    ///     This facility may be registered within your web application to automatically look for and close
    ///     WCF connections.  This eliminates all the redundant code for closing the connection and aborting
    ///     if any appropriate exceptions are encountered.  See documentation for setting up and using this
    ///     Castle facility.
    /// </summary>
    public class WcfSessionFacility : IFacility
    {
        public const string ManageWcfSessionsKey = "ManageWcfSessions";

        public void Init(IKernel kernel, IConfiguration facilityConfig)
        {
            kernel.ComponentDestroyed += KernelComponentDestroyed;
        }

        public void Terminate()
        {
        }

        private static void KernelComponentDestroyed(ComponentModel model, object instance)
        {
            var shouldManage = false;
            var value = model.ExtendedProperties[ManageWcfSessionsKey];

            if (value != null)
            {
                shouldManage = (bool)value;
            }

            if (!shouldManage)
            {
                return;
            }

            var communicationObject = instance as ICommunicationObject;

            if (communicationObject != null)
            {
                try
                {
                    if (communicationObject.State != CommunicationState.Faulted)
                    {
                        communicationObject.Close();
                    }
                    else
                    {
                        communicationObject.Abort();
                    }
                }
                catch (CommunicationException)
                {
                    communicationObject.Abort();
                }
                catch (TimeoutException)
                {
                    communicationObject.Abort();
                }
                catch (Exception)
                {
                    communicationObject.Abort();
                    throw;
                }
            }
        }
    }
}