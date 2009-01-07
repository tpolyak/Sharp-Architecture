using System.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Cfg;
using SharpArch.Core;
using FluentNHibernate;
using System.Reflection;

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
        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies) {
            return Init(storage, mappingAssemblies, null, null, null);
        }

        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, string cfgFile) {
            return Init(storage, mappingAssemblies, cfgFile, null, null);
        }

        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, IDictionary<string ,string> cfgProperties) {
            return Init(storage, mappingAssemblies, null, cfgProperties, null);
        }

        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, string cfgFile, string validatorCfgFile) {
            return Init(storage, mappingAssemblies, cfgFile, null, validatorCfgFile);
        }

        public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, string cfgFile, IDictionary<string ,string> cfgProperties, string validatorCfgFile) {
            Check.Require(storage != null, "storage mechanism was null but must be provided");
            
            Configuration cfg = ConfigureNHibernate(cfgFile, cfgProperties);
            ConfigureNHibernateValidator(cfg, validatorCfgFile);
            AddMappingAssembliesTo(cfg, mappingAssemblies);

 			SessionFactory = cfg.BuildSessionFactory();
 			Storage = storage;

            return cfg;
 		}

        private static void AddMappingAssembliesTo(Configuration cfg, ICollection<string> mappingAssemblies) {
            Check.Require(mappingAssemblies != null && mappingAssemblies.Count >= 1,
                "mappingAssemblies must be provided as a string array of assembly names which contain mapping artifacts. " +
                "The artifacts themselves may be HBMs or ClassMaps.  You may optionally include '.dll' on the assembly name.");

            foreach (string mappingAssembly in mappingAssemblies) {
                string loadReadyAssemblyName = (mappingAssembly.IndexOf(".dll") == -1)
                    ? mappingAssembly.Trim() + ".dll"
                    : mappingAssembly.Trim();

                Assembly assemblyToInclude = Assembly.LoadFrom(loadReadyAssemblyName);
                // Looks for any HBMs
                cfg.AddAssembly(assemblyToInclude);
                // Looks for any Fluent NHibernate ClassMaps
                cfg.AddMappingsFromAssembly(assemblyToInclude);
            }
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

        public static ISessionFactory SessionFactory { get; set; }
		public static ISessionStorage Storage { get; set; }

		public static ISession Current {
			get {
				ISession session = Storage.Session;
				
                if (session == null) {
					session = SessionFactory.OpenSession();
					Storage.Session = session;
				}

                return session;
			}
		}
	}
}
