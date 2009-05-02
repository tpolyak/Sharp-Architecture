using System.ServiceModel;
using SharpArch.Data.NHibernate;
using NHibernate;
using System;

namespace SharpArch.Wcf.NHibernate
{
    public class WcfSessionStorage : ISessionStorage
    {
        public ISession Session {
            get {
                SessionInstanceExtension nHibernateSessionInstanceExtension =
                    OperationContext.Current.InstanceContext.Extensions.Find<SessionInstanceExtension>();

                if (nHibernateSessionInstanceExtension == null)
                    return null;
                
                return nHibernateSessionInstanceExtension.Session;
            }
            set {
                SessionInstanceExtension nHibernateSessionInstanceExtension =
                    OperationContext.Current.InstanceContext.Extensions.Find<SessionInstanceExtension>();

                if (nHibernateSessionInstanceExtension == null)
                    return;
                
                nHibernateSessionInstanceExtension.Session = value;
            }
        }

        public string FactoryKey {
            get {
                return NHibernateSession.DefaultFactoryKey;
            }
        }
    }
}
