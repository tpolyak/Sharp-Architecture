using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpArch.Data.NHibernate
{
    /// <summary>
    /// Invoked by Global.asax.cx, or wherever you can to initialize NHibernate, to guarentee that 
    /// NHibernate is only initialized once while working in IIS 7 integrated mode.  But note 
    /// that this is not web specific, although that is the realm that prompted its creation.
    /// 
    /// In a web context, it should be invoked from Application_BeginRequest with the NHibernateSession.Init()
    /// function being passed as a parameter to InitializeNHiberate()
    /// </summary>
    public class NHibernateInitializer
    {
        private static readonly object syncLock = new object();
        private static NHibernateInitializer instance;

        protected NHibernateInitializer() { }

        private bool nHibernateSessionIsLoaded = false;

        public static NHibernateInitializer Instance() {
            if (instance == null) {
                lock (syncLock) {
                    if (instance == null) {
                        instance = new NHibernateInitializer();
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// This is the method which should be given the call to intialize the NHibernateSession; e.g.,
        /// NHibernateInitializer.Instance().InitializeNHibernateOnce(() => InitializeNHibernateSession());
        /// where InitializeNHibernateSession() is a method which calls NHibernateSession.Init()
        /// </summary>
        /// <param name="initMethod"></param>
        public void InitializeNHibernateOnce(Action initMethod) {
            lock (syncLock) {
                if (!nHibernateSessionIsLoaded) {
                    initMethod();
                    nHibernateSessionIsLoaded = true;
                }
            }
        }
    }
}
