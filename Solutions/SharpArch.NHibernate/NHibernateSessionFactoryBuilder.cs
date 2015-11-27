namespace SharpArch.NHibernate
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Domain;
    using global::FluentNHibernate.Automapping;
    using global::FluentNHibernate.Cfg;
    using global::FluentNHibernate.Cfg.Db;
    using global::NHibernate;
    using global::NHibernate.Cfg;
    using NHibernateValidator;

    /// <summary>
    ///     Creates NHibernate SessionFactory <see cref="ISessionFactory" />
    /// </summary>
    /// <remarks>
    ///     Transient object, session factory must be redistered as singletone in DI Container.
    /// </remarks>
    public class NHibernateSessionFactoryBuilder
    {
        /// <summary>
        /// Default configuration file name.
        /// </summary>
        public const string DefaultNHibernateConfigFileName = "Hibernate.cfg.xml";

        public static readonly string DefaultConfigurationName = "nhibernate.current_session";

        private INHibernateConfigurationCache configurationCache;
        
        private AutoPersistenceModel autoPersistenceModel;
        private string configFile;
        private readonly List<string> mappingAssemblies;
        private IPersistenceConfigurer persistenceConfigurer;
        private IDictionary<string, string> properties;
        private bool useDataAnnotationValidators;
        Action<Configuration> exposeConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateSessionFactoryBuilder"/> class.
        /// </summary>
        public NHibernateSessionFactoryBuilder()
        {
            this.configurationCache = NullNHibernateConfigurationCache.Null;
            this.mappingAssemblies = new List<string>();
        }


        /// <summary>
        /// Creates the session factory.
        /// </summary>
        /// <returns> NHibernate session factory <see cref="ISessionFactory"/>.</returns>
        public ISessionFactory BuildSessionFactory()
        {
            var configuration = BuildConfiguration();
            return configuration.BuildSessionFactory();
        }


        /// <summary>
        /// Builds NHibernate configuration.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Any changes made to configuration object <b>will not be persisted</b> in configuration cache.
        /// This can be usefull to make dynamic changes to configuration or in case changes cannot be serialized (e.g. event listeners are not marked with <see cref="SerializableAttribute"/>.
        /// </para>
        /// <para>
        /// To make persistent changes use <seealso cref="ExposeConfiguration"/>.
        /// </para>
        /// </remarks>
        public Configuration BuildConfiguration()
        {
            var configuration = configurationCache.LoadConfiguration(DefaultConfigurationName, configFile, mappingAssemblies);

            if (configuration == null)
            {
                configuration = LoadExternalConfiguration();
                configuration = ApplyCustomSettings(configuration);
                configurationCache.SaveConfiguration(DefaultConfigurationName, configuration);
            }
            return configuration;
        }

        /// <summary>
        /// Allows to alter configuration before creating NHibernate configuration.
        /// </summary>
        /// <remarks>
        /// Changes to configuration will be persisted in configuration cache, if it is enabled.
        /// In case changes must not be persisted in cache, they must be applied after <seealso cref="BuildConfiguration"/>.
        /// </remarks>
        public NHibernateSessionFactoryBuilder ExposeConfiguration(Action<Configuration> config)
        {
            Check.Require(config != null, "Please provide callback.");
            this.exposeConfiguration = config;

            return this;
        }

        private bool ShouldExposeConfiguration()
        {
            return exposeConfiguration != null;
        }

        public NHibernateSessionFactoryBuilder UseConfigurationCache(INHibernateConfigurationCache configurationCache)
        {
            Check.Require(configurationCache != null, "Please provide configuration cache instance.");
            this.configurationCache = configurationCache;
            return this;
        }

        public NHibernateSessionFactoryBuilder AddMappingAssemblies(IEnumerable<string> mappingAssemblies)
        {
            Check.Require(mappingAssemblies != null, "Please specify mapping assemblies.");
            this.mappingAssemblies.AddRange(mappingAssemblies);
            return this;
        }

        public NHibernateSessionFactoryBuilder UseAutoPersitenceModel(AutoPersistenceModel autoPersistenceModel)
        {
            Check.Require(autoPersistenceModel != null);
            this.autoPersistenceModel = autoPersistenceModel;
            return this;
        }

        public NHibernateSessionFactoryBuilder UseProperties(IDictionary<string, string> properties)
        {
            Check.Require(properties != null);
            this.properties = properties;

            return this;
        }


        public NHibernateSessionFactoryBuilder UseDataAnnotationValidators(bool addDataAnnotatonValidators)
        {
            this.useDataAnnotationValidators = addDataAnnotatonValidators;
            return this;
        }

        public NHibernateSessionFactoryBuilder UseConfigFile(string nhibernateConfigFile)
        {
            Check.Require(!string.IsNullOrEmpty(nhibernateConfigFile), "NHibernate config file must be specified");
            configFile = nhibernateConfigFile;

            return this;
        }

        public NHibernateSessionFactoryBuilder UsePersistenceConfigurer(IPersistenceConfigurer persistenceConfigurer)
        {
            Check.Require(persistenceConfigurer != null);
            this.persistenceConfigurer = persistenceConfigurer;
            return this;
        }

        private Configuration ApplyCustomSettings(Configuration cfg)
        {
            var fluentConfig = Fluently.Configure(cfg);
            if (persistenceConfigurer != null)
            {
                fluentConfig.Database(persistenceConfigurer);
            }

            fluentConfig.Mappings(m =>
            {
                foreach (string mappingAssembly in this.mappingAssemblies)
                {
                    Assembly assembly = Assembly.LoadFrom(MakeLoadReadyAssemblyName(mappingAssembly));

                    m.HbmMappings.AddFromAssembly(assembly);
                    m.FluentMappings.AddFromAssembly(assembly).Conventions.AddAssembly(assembly);
                }

                if (autoPersistenceModel != null)
                {
                    m.AutoMappings.Add(autoPersistenceModel);
                }
            });

            if (this.useDataAnnotationValidators || ShouldExposeConfiguration())
            {
                fluentConfig.ExposeConfiguration(AddValidatorsAndExposeConfiguration);
            }
            return fluentConfig.BuildConfiguration();
        }

        void AddValidatorsAndExposeConfiguration(Configuration e)
        {
            if (useDataAnnotationValidators)
            {
                e.EventListeners.PreInsertEventListeners = InsertFirst(e.EventListeners.PreInsertEventListeners, new DataAnnotationsEventListener());
                e.EventListeners.PreUpdateEventListeners = InsertFirst(e.EventListeners.PreUpdateEventListeners, new DataAnnotationsEventListener());
            }
            if (ShouldExposeConfiguration())
            {
                exposeConfiguration(e);
            }
        }

        /// <summary>
        ///     Loads configuration from properties dictionary and from external file if avaiable.
        /// </summary>
        /// <returns></returns>
        private Configuration LoadExternalConfiguration()
        {
            var cfg = new Configuration();
            if (properties != null && properties.Any())
            {
                cfg.AddProperties(properties);
            }
            if (!string.IsNullOrEmpty(configFile))
            {
                return cfg.Configure(configFile);
            }
            if (File.Exists(DefaultNHibernateConfigFileName))
            {
                return cfg.Configure();
            }
            return cfg;
        }

        private static T[] InsertFirst<T>(T[] array, T item)
        {
            if (array == null)
            {
                return new[] {item};
            }

            var items = new List<T> {item};
            items.AddRange(array);
            return items.ToArray();
        }

        private static string MakeLoadReadyAssemblyName(string assemblyName)
        {
            return (assemblyName.IndexOf(".dll", StringComparison.OrdinalIgnoreCase) == -1)
                ? assemblyName.Trim() + ".dll"
                : assemblyName.Trim();
        }
    }
}
