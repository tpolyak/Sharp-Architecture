using System.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Cfg;
using SharpArch.Core;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using System.Reflection;
using FluentNHibernate.AutoMap;
using System.Linq;

namespace SharpArch.Data.NHibernate
{
	public static class NHibernateSession
    {
        static NHibernateSession() {
            SessionFactories = new Dictionary<string, ISessionFactory>();
            Storages = new Dictionary<string, ISessionStorage>();
        }

        #region Init() overloads

        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies) {
            return Init(storage, mappingAssemblies, null, null, null, null);
        }

        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, string cfgFile) {
            return Init(storage, mappingAssemblies, null, cfgFile, null, null);
        }

        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, IDictionary<string ,string> cfgProperties) {
            return Init(storage, mappingAssemblies, null, null, cfgProperties, null);
        }

        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, string cfgFile, string validatorCfgFile) {
            return Init(storage, mappingAssemblies, null, cfgFile, null, validatorCfgFile);
        }

        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel) {
            return Init(storage, mappingAssemblies, autoPersistenceModel, null, null, null);
        }

        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel, string cfgFile) {
            return Init(storage, mappingAssemblies, autoPersistenceModel, cfgFile, null, null);
        }

        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel, IDictionary<string, string> cfgProperties) {
            return Init(storage, mappingAssemblies, autoPersistenceModel, null, cfgProperties, null);
        }

        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel, string cfgFile, string validatorCfgFile) {
            return Init(storage, mappingAssemblies, autoPersistenceModel, cfgFile, null, validatorCfgFile);
        }

        #endregion

        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, 
            AutoPersistenceModel autoPersistenceModel, string cfgFile, 
            IDictionary<string, string> cfgProperties, string validatorCfgFile) {

            Check.Require(storage != null, "storage mechanism was null but must be provided");
            Check.Require(!SessionFactories.ContainsKey(storage.FactoryKey), 
                "A session factory has already been configured with the key of " + storage.FactoryKey);

            Configuration cfg = ConfigureNHibernate(cfgFile, cfgProperties);
            ConfigureNHibernateValidator(cfg, validatorCfgFile);

            SessionFactories.Add(
                storage.FactoryKey, 
                CreateSessionFactoryFor(mappingAssemblies, autoPersistenceModel, cfg));

            Storages.Add(storage.FactoryKey, storage);

            return cfg;
        }

        private static ISessionFactory CreateSessionFactoryFor(string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel, Configuration cfg) {
            return Fluently.Configure(cfg)
                .Mappings(m => {
                    foreach (var mappingAssembly in mappingAssemblies) {
                        var assembly = Assembly.LoadFrom(MakeLoadReadyAssemblyName(mappingAssembly));

                        m.HbmMappings.AddFromAssembly(assembly);
                        m.FluentMappings.AddFromAssembly(assembly);
                    }

                    if (autoPersistenceModel != null) {
                        m.AutoMappings.Add(autoPersistenceModel);
                    }
                })
                .BuildSessionFactory();
        }

        public static void RegisterInterceptor(IInterceptor interceptor) {
            Check.Require(interceptor != null, "interceptor may not be null");

            RegisteredInterceptor = interceptor;
        }

        /// <summary>
        /// Provides access to the <see cref="ISessionFactory" /> assuming you're communicating
        /// with one database; otherwise, you should use <see cref="SessionFactories" />.
        /// </summary>
        public static ISessionFactory SessionFactory {
            get {
                Check.Require(SessionFactories.Count <= 1, "The SessionFactory getter may only " +
                    "be invoked if you only have zero or one NHibernate session factories; i.e., " +
                    "you're only communicating with one database or have not yet set the " +
                    "SessionFactory.  Since you've configured multiple SesionFactores for " + 
                    "communications with multiple databases, you should instead use SessionFactories");

                return SessionFactories.Values.FirstOrDefault();
            }
            set {
                Check.Require(SessionFactories.Count <= 1, 
                    "The SessionFactory setter may only be invoked if you have not yet set an NHibernate " +
                    "session factory or are reassigning the one, and only one, that exists; i.e., you're only " +
                    "communicating with one database.  Since you've already created more than one session factory, " + 
                    "you should instead use the SessionFactories property.");

                if (value != null) {
                    if (SessionFactories.Count == 0) {
                        SessionFactories.Add(DefaultFactoryKey, value);
                    }
                    // Replace existing session factory if different
                    else if (! value.Equals(SessionFactories.First())) {
                        RemoveOneAndOnlySessionFactory();
                        SessionFactories.Add(DefaultFactoryKey, value);
                    }
                }
                else {
                    if (SessionFactories.Count > 0) {
                        RemoveOneAndOnlySessionFactory();
                    }
                }
            }
        }

        public static ISessionStorage Storage {
            get {
                Check.Require(Storages.Count <= 1, "The Storage getter may only " +
                    "be invoked if you only have zero or one NHibernate session storages; i.e., " +
                    "you're only communicating with one database or have not yet set the " +
                    "Storage.  Since you've configured multiple Storages for " +
                    "communications with multiple databases, you should instead use the Storages property.");

                return Storages.Values.FirstOrDefault();
            }
            set {
                Check.Require(Storages.Count <= 1, 
                    "The Storages setter may only be invoked if you have not yet set an NHibernate " +
                    "session storage or reassigning the one, and only one, that exists; i.e., you're only " +
                    "communicating with one database. Since you've already created more than one session storage, " +
                    "you should instead use the Storages property.");

                if (value != null) {
                    if (Storages.Count == 0) {
                        Storages.Add(DefaultFactoryKey, value);
                    }
                    // Replace existing session storage if different
                    else if (!value.Equals(Storages.First())) {
                        RemoveOneAndOnlySessionStorage();
                        Storages.Add(DefaultFactoryKey, value);
                    }
                }
                else {
                    if (Storages.Count > 0) {
                        RemoveOneAndOnlySessionStorage();
                    }
                }
            }
        }

        private static void RemoveOneAndOnlySessionFactory() {
            Check.Require(SessionFactories.Count <= 1, "This may only be invoked if SessionFactories " +
                "contains 1 or fewer items; it currently contains " + SessionFactories.Count);

            if (SessionFactories.Count > 0) {
                ISessionFactory factory = SessionFactories.Values.First();
                factory.Dispose();
                factory = null;
                SessionFactories.Clear();
            }
        }

        private static void RemoveOneAndOnlySessionStorage() {
            Check.Require(Storages.Count <= 1, "This may only be invoked if Storages " +
                "contains 1 or fewer items; it currently contains " + Storages.Count);

            if (Storages.Count > 0) {
                ISessionStorage storage = Storages.Values.First();
                storage = null;
                Storages.Clear();
            }
        }

        public static bool IsConfiguredForMultipleDatabases() {
            return SessionFactories.Count > 1;
        }

        /// <summary>
		/// Used to get the current NHibernate session if you're communicating with a single database.
        /// When communicating with multiple databases, invoke <see cref="CurrentFor()" /> instead.
		/// </summary>
        public static ISession Current {
			get {
                Check.Require(Storages.Count == 1, "The NHibernateSession.Current property may " +
                    "only be invoked if you only have one NHibernate session factory; i.e., you're " + 
                    "only communicating with one database.  Since you're configured communications " +
                    "with multiple databases, you should instead call Current(string factoryKey)");

                return CurrentFor(DefaultFactoryKey);
			}
		}

        /// <summary>
        /// Maintains a dictionary of NHibernate session factories, one per database.  The key is 
        /// the "factory key" used to look up the associated database, and used to decorate respective
        /// repositories.  If only one database is being used, this dictionary contains a single
        /// factory with a key of <see cref="DefaultFactoryKey" />.
        /// </summary>
        public static IDictionary<string, ISessionFactory> SessionFactories { get; set; }

        /// <summary>
        /// Maintains a dictionary of NHibernate session storages, one per database.  The key is 
        /// the "factory key" used to look up the associated database, and used to decorate respective
        /// repositories.  If only one database is being used, this dictionary contains a single
        /// session storage with a key of <see cref="DefaultFactoryKey" />.
        /// </summary>
        public static IDictionary<string, ISessionStorage> Storages { get; set; }

        /// <summary>
        /// The default factory key used if only one database is being communicated with.
        /// </summary>
        public static readonly string DefaultFactoryKey = "nhibernate.current_session";

        /// <summary>
        /// Used to get the current NHibernate session associated with a factory key; i.e., the key 
        /// associated with an NHibernate session factory for a specific database.
        /// 
        /// If you're only communicating with one database, you should call <see cref="Current" /> instead,
        /// although you're certainly welcome to call this if you have the factory key available.
        /// </summary>
        public static ISession CurrentFor(string factoryKey) {
            Check.Require(! string.IsNullOrEmpty(factoryKey), "factoryKey may not be null or empty");
            Check.Require(Storages.ContainsKey(factoryKey), "An ISessionStorage does not exist with a factory key of " + factoryKey);
            Check.Require(SessionFactories.ContainsKey(factoryKey), "An ISessionFactory does not exist with a factory key of " + factoryKey);

            ISession session = Storages[factoryKey].Session;

            if (session == null) {
                if (RegisteredInterceptor != null) {
                    session = SessionFactories[factoryKey].OpenSession(RegisteredInterceptor);
                }
                else {
                    session = SessionFactories[factoryKey].OpenSession();
                }

                Storages[factoryKey].Session = session;
            }

            return session;
        }

        private static string MakeLoadReadyAssemblyName(string assemblyName) {
            return (assemblyName.IndexOf(".dll") == -1)
				? assemblyName.Trim() + ".dll"
				: assemblyName.Trim();
        }

        private static Configuration ConfigureNHibernate(string cfgFile, IDictionary<string, string> cfgProperties) {
            Configuration cfg = new Configuration();

            if (cfgProperties != null)
                cfg.AddProperties(cfgProperties);

            if (string.IsNullOrEmpty(cfgFile))
                return cfg.Configure();

            return cfg.Configure(cfgFile);
        }

        private static void ConfigureNHibernateValidator(Configuration cfg, string validatorCfgFile) {
            ValidatorEngine engine = new ValidatorEngine();

            if (string.IsNullOrEmpty(validatorCfgFile))
                engine.Configure();
            else
                engine.Configure(validatorCfgFile);

            // Register validation listeners with the current NHib configuration
            ValidatorInitializer.Initialize(cfg, engine);
        }

        private static IInterceptor RegisteredInterceptor;
    }
}
