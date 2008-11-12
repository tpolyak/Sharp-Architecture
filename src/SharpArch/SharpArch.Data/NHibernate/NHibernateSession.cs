using NHibernate;
using NHibernate.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Cfg;
using SharpArch.Core;

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
        public static void Init(ISessionStorage storage) {
            Init(storage, null, null);
        }

        public static void Init(ISessionStorage storage, string cfgFile) {
            Init(storage, cfgFile, null);
        }

        public static void Init(ISessionStorage storage, string cfgFile, string validatorCfgFile) {
            Check.Require(storage != null, "storage mechanism was null but must be provided");
            
            Configuration cfg = ConfigureNHibernate(cfgFile);
            ConfigureNHibernateValidator(cfg, validatorCfgFile);

 			SessionFactory = cfg.BuildSessionFactory();
 			Storage = storage;
 		}

        private static Configuration ConfigureNHibernate(string cfgFile) {
            Configuration cfg = new Configuration();

            if (string.IsNullOrEmpty(cfgFile))
                return cfg.Configure();
            else
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
