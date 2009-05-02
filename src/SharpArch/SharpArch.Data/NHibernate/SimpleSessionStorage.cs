using NHibernate;

namespace SharpArch.Data.NHibernate
{
    public class SimpleSessionStorage : ISessionStorage
    {
        /// <summary>
        /// Creates a SimpleSessionStorage instance for a single DB session.
        /// </summary>
        public SimpleSessionStorage() : this(null) { }

        /// <summary>
        /// Creates a SimpleSessionStorage instance for multiple databases. Each 
        /// SimpleSessionStorage must be given a unique factoryKey for each database
        /// that it will be connecting to.  The factoryKey will be referenced when
        /// decorating repositories with <see cref="SessionFactoryAttribute"/>.
        /// </summary>
        /// <param name="factoryKey">An example would be "MyOtherDb"</param>
        public SimpleSessionStorage(string factoryKey) {
            if (!string.IsNullOrEmpty(factoryKey)) {
                this.factoryKey = factoryKey;
            }
            // If a unique session identifier was not provided, then set a default identifier
            else {
                this.factoryKey = NHibernateSession.DefaultFactoryKey;
            }
        }

        public ISession Session { get; set; }

        public string FactoryKey {
            get {
                return factoryKey;
            }
        }

        private string factoryKey;
    }
}
