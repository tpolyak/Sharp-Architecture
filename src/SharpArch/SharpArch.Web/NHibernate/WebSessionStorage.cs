using System;
using System.Web;
using NHibernate;
using SharpArch.Data.NHibernate;

namespace SharpArch.Web.NHibernate
{
    public class WebSessionStorage : ISessionStorage
    {
        /// <summary>
        /// Creates a WebSessionStorage instance for a single DB session.
        /// </summary>
        public WebSessionStorage(HttpApplication app) : this(app, null) { }

        /// <summary>
        /// Creates a WebSessionStorage instance for multiple databases. Each 
        /// WebSessionStorage must be given a unique factoryKey for each database
        /// that it will be connecting to.  The factoryKey will be referenced when
        /// decorating repositories with <see cref="SessionFactoryAttribute"/>.
        /// </summary>
        /// <param name="factoryKey">An example would be "MyOtherDb"</param>
        public WebSessionStorage(HttpApplication app, string factoryKey) {
            app.EndRequest += Application_EndRequest;

            if (!string.IsNullOrEmpty(factoryKey)) {
                this.factoryKey = factoryKey;
            }
            // If a unique session identifier was not provided, then set a default identifier
            else {
                this.factoryKey = NHibernateSession.DefaultFactoryKey;
            }
        }

        public ISession Session {
            get {
                HttpContext context = HttpContext.Current;
                ISession session = context.Items[factoryKey] as ISession;
                return session;
            }
            set {
                HttpContext context = HttpContext.Current;
                context.Items[factoryKey] = value;
            }
        }

        public string FactoryKey {
            get { return factoryKey; }
        }

        private void Application_EndRequest(object sender, EventArgs e) {
            ISession session = Session;

            if (session != null) {
                session.Close();
                HttpContext context = HttpContext.Current;
                context.Items.Remove(factoryKey);
            }
        }

        private string factoryKey;
    }
}
