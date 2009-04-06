using System;
using Castle.Windsor;
using System.Web;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using SharpArch.Data.NHibernate;
using SharpArch.Wcf.NHibernate;
using Northwind.Data.NHibernateMaps;
using Northwind.Wcf.Web.CastleWindsor;

namespace Northwind.Wcf.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e) {
            InitializeServiceLocator();
        }

        /// <summary>
        /// Instantiate the container and add components.
        /// </summary>
        protected virtual void InitializeServiceLocator() {
            IWindsorContainer container = new WindsorContainer();
            ComponentRegistrar.AddComponentsTo(container);

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }

        /// <summary>
        /// Due to issues on IIS7, the NHibernate initialization must occur in Init().
        /// But Init() may be invoked more than once; accordingly, we introduce a thread-safe
        /// mechanism to ensure it's only initialized once.
        /// 
        /// See http://msdn.microsoft.com/en-us/magazine/cc188793.aspx for explanation details.
        /// </summary>
        public override void Init() {
            base.Init();

            // Only allow the NHibernate session to be initialized once
            if (!wasNHibernateInitialized) {
                lock (lockObject) {
                    if (!wasNHibernateInitialized) {
                        // Note the use of WcfSessionStorage below...very important!
                        NHibernateSession.Init(new WcfSessionStorage(),
                            new string[] { Server.MapPath("~/bin/Northwind.Data.dll") },
                            new AutoPersistenceModelGenerator().Generate(),
                            Server.MapPath("~/NHibernate.config"));

                        wasNHibernateInitialized = true;
                    }
                }
            }
        }

        private static bool wasNHibernateInitialized = false;

        /// <summary>
        /// Private, static object used only for synchronization
        /// </summary>
        private static object lockObject = new object();
    }
}