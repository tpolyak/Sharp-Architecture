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

namespace SharpArch.Data.NHibernate
{
	public interface ISessionStorage
	{
		ISession Session { get; set; }
	}

	public class SimpleSessionStorage : ISessionStorage
	{
	    public ISession Session { get; set; }
	}

	public static class NHibernateSession
    {
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

        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel, string cfgFile, IDictionary<string, string> cfgProperties, string validatorCfgFile) {
            Check.Require(storage != null, "storage mechanism was null but must be provided");

            Configuration cfg = ConfigureNHibernate(cfgFile, cfgProperties);
            ConfigureNHibernateValidator(cfg, validatorCfgFile); 

            SessionFactory = Fluently.Configure(cfg)
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

            Storage = storage;

            return cfg;
        }

        public static void RegisterInterceptor(IInterceptor interceptor) {
            Check.Require(interceptor != null, "interceptor may not be null");

            RegisteredInterceptor = interceptor;
        }

        public static ISessionFactory SessionFactory { get; set; }
		public static ISessionStorage Storage { get; set; }

		public static ISession Current {
			get {
				ISession session = Storage.Session;
				
                if (session == null) {
                    if (RegisteredInterceptor != null) {
                        session = SessionFactory.OpenSession(RegisteredInterceptor);
                    }
                    else {
                        session = SessionFactory.OpenSession();
                    }

                    Storage.Session = session;
				}

                return session;
			}
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
