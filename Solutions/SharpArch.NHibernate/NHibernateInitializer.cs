namespace SharpArch.NHibernate
{
    using System;

    /// <summary>
    ///     Invoked by Global.asax.cx, or wherever you can to initialize NHibernate, to guarentee that 
    ///     NHibernate is only initialized once while working in IIS 7 integrated mode.  But note 
    ///     that this is not web specific, although that is the realm that prompted its creation.
    /// 
    ///     In a web context, it should be invoked from Application_BeginRequest with the NHibernateSession.Init()
    ///     function being passed as a parameter to InitializeNHiberate()
    /// </summary>
    public class NHibernateInitializer
    {
        private static readonly object SyncLock = new object();

        private static NHibernateInitializer instance;

        private bool nHibernateSessionIsLoaded;

        protected NHibernateInitializer()
        {
        }

        public static NHibernateInitializer Instance()
        {
            if (instance == null)
            {
                lock (SyncLock)
                {
                    if (instance == null)
                    {
                        instance = new NHibernateInitializer();
                    }
                }
            }

            return instance;
        }

        /// <summary>
        ///     This is the method which should be given the call to intialize the NHibernateSession; e.g.,
        ///     NHibernateInitializer.Instance().InitializeNHibernateOnce(() => InitializeNHibernateSession());
        ///     where InitializeNHibernateSession() is a method which calls NHibernateSession.Init()
        /// </summary>
        /// <param name = "initMethod"></param>
        public void InitializeNHibernateOnce(Action initMethod)
        {
            lock (SyncLock)
            {
                if (!this.nHibernateSessionIsLoaded)
                {
                    initMethod();
                    this.nHibernateSessionIsLoaded = true;
                }
            }
        }
    }
}