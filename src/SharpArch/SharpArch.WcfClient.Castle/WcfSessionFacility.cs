using System;
using Castle.MicroKernel.ModelBuilder;
using Castle.MicroKernel;
using Castle.Core;
using Castle.Core.Configuration;
using System.ServiceModel;

namespace SharpArch.WcfClient.Castle
{
	public class WcfSessionFacility : IFacility
	{
		public const string ManageWcfSessionsKey = "ManageWcfSessions";

		#region IFacility Members

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

			ICommunicationObject obj = instance as ICommunicationObject;
			if (obj != null) {
				try {
					if (obj.State != CommunicationState.Faulted) {
						obj.Close();
					}
					else {
						obj.Abort();
					}
				}
				catch (CommunicationException) {
					obj.Abort();
				}
				catch (System.TimeoutException) {
					obj.Abort();
				}
				catch (Exception) {
					obj.Abort();
					throw;
				}
			}
		}

		public void Terminate() {
		}

		#endregion
	}

}