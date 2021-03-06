﻿using System.Collections.Generic;
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
using FluentNHibernate.Cfg.Db;
using System;

namespace SharpArch.Data.NHibernate
{
	public static class NHibernateSession
	{
		#region Initialization & Configuration

		#region Init() overloads

		public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies)
		{
			return Init(storage, mappingAssemblies, null, null, null, null, null);
		}

		public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, string cfgFile)
		{
			return Init(storage, mappingAssemblies, null, cfgFile, null, null, null);
		}

		public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, IDictionary<string, string> cfgProperties)
		{
			return Init(storage, mappingAssemblies, null, null, cfgProperties, null, null);
		}

		public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, string cfgFile, string validatorCfgFile)
		{
			return Init(storage, mappingAssemblies, null, cfgFile, null, validatorCfgFile, null);
		}

		[CLSCompliant(false)]
		public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel)
		{
			return Init(storage, mappingAssemblies, autoPersistenceModel, null, null, null, null);
		}

		[CLSCompliant(false)]
		public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel, string cfgFile)
		{
			return Init(storage, mappingAssemblies, autoPersistenceModel, cfgFile, null, null, null);
		}

		[CLSCompliant(false)]
		public static Configuration Init(ISessionStorage storage, string[] mappingAssemblies, AutoPersistenceModel autoPersistenceModel, IDictionary<string, string> cfgProperties)
		{
			return Init(storage, mappingAssemblies, autoPersistenceModel, null, cfgProperties, null, null);
		}

		[CLSCompliant(false)]
		public static Configuration Init(
			ISessionStorage storage, 
			string[] mappingAssemblies,
			AutoPersistenceModel autoPersistenceModel,
			string cfgFile,
			string validatorCfgFile)
		{
			return Init(storage, mappingAssemblies, autoPersistenceModel, cfgFile, null, validatorCfgFile, null);
		}

		[CLSCompliant(false)]
		public static Configuration Init(
			ISessionStorage storage, 
			string[] mappingAssemblies,
			AutoPersistenceModel autoPersistenceModel,
			string cfgFile,
			IDictionary<string, string> cfgProperties,
			string validatorCfgFile)
		{
			return Init(storage, mappingAssemblies, autoPersistenceModel, cfgFile, null, validatorCfgFile, null);
		}

		[CLSCompliant(false)]
		public static Configuration Init(
			ISessionStorage storage,
			string[] mappingAssemblies,
			AutoPersistenceModel autoPersistenceModel,
			string cfgFile,
			IDictionary<string, string> cfgProperties,
			string validatorCfgFile,
			IPersistenceConfigurer persistenceConfigurer)
		{
			InitStorage(storage);
			return AddConfiguration(DefaultFactoryKey, mappingAssemblies, autoPersistenceModel, cfgFile, cfgProperties, validatorCfgFile, persistenceConfigurer);
		}

		#endregion

		public static void InitStorage(ISessionStorage storage)
		{
			Check.Require(storage != null, "storage mechanism was null but must be provided");
			Check.Require(Storage == null, "A storage mechanism was has already been configured for this application");
			Storage = storage;
		}

		[CLSCompliant(false)]
		public static Configuration AddConfiguration(
			string factoryKey,
			string[] mappingAssemblies,
			AutoPersistenceModel autoPersistenceModel,
			string cfgFile,
			IDictionary<string, string> cfgProperties,
			string validatorCfgFile,
			IPersistenceConfigurer persistenceConfigurer)
		{
			Configuration cfg = ConfigureNHibernate(cfgFile, cfgProperties);
			ConfigureNHibernateValidator(cfg, validatorCfgFile);

			Check.Require(!sessionFactories.ContainsKey(factoryKey),
				"A session factory has already been configured with the key of " + factoryKey);

			sessionFactories.Add(
				factoryKey,
				CreateSessionFactoryFor(mappingAssemblies, autoPersistenceModel, cfg, persistenceConfigurer));

			return cfg;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Provides an access to configured<see cref="ValidatorEngine"/>.
		/// </summary>
		/// <value>The validator engine.</value>
		public static ValidatorEngine ValidatorEngine { get; set; }

		/// <summary>
		/// Used to get the current NHibernate session if you're communicating with a single database.
		/// When communicating with multiple databases, invoke <see cref="CurrentFor()" /> instead.
		/// </summary>
		public static ISession Current
		{
			get
			{
				Check.Require(!IsConfiguredForMultipleDatabases(),
					"The NHibernateSession.Current property may " +
					"only be invoked if you only have one NHibernate session factory; i.e., you're " +
					"only communicating with one database.  Since you're configured communications " +
					"with multiple databases, you should instead call CurrentFor(string factoryKey)");

				return CurrentFor(DefaultFactoryKey);
			}
		}

		#endregion

		#region Public Methods
		public static void RegisterInterceptor(IInterceptor interceptor)
		{
			Check.Require(interceptor != null, "interceptor may not be null");

			RegisteredInterceptor = interceptor;
		}

		public static bool IsConfiguredForMultipleDatabases()
		{
			return sessionFactories.Count > 1;
		}

		/// <summary>
		/// Used to get the current NHibernate session associated with a factory key; i.e., the key 
		/// associated with an NHibernate session factory for a specific database.
		/// 
		/// If you're only communicating with one database, you should call <see cref="Current" /> instead,
		/// although you're certainly welcome to call this if you have the factory key available.
		/// </summary>
		public static ISession CurrentFor(string factoryKey)
		{
			Check.Require(!string.IsNullOrEmpty(factoryKey), "factoryKey may not be null or empty");
			Check.Require(Storage != null, "An ISessionStorage has not been configured");
			Check.Require(sessionFactories.ContainsKey(factoryKey), "An ISessionFactory does not exist with a factory key of " + factoryKey);

			ISession session = Storage.GetSessionForKey(factoryKey);

			if (session == null)
			{
				if (RegisteredInterceptor != null)
				{
					session = sessionFactories[factoryKey].OpenSession(RegisteredInterceptor);
				}
				else
				{
					session = sessionFactories[factoryKey].OpenSession();
				}

				Storage.SetSessionForKey(factoryKey, session);
			}

			return session;
		}

		/// <summary>
		/// This method is used by application-specific session storage implementations
		/// and unit tests. Its job is to walk thru existing cached sessions and Close() each one.
		/// </summary>
		public static void CloseAllSessions()
		{
			foreach (ISession session in Storage.GetAllSessions())
			{
				if (session.IsOpen)
					session.Close();
			}
		}

		/// <summary>
		/// To facilitate unit testing, this method will reset this object back to its
		/// original state before it was configured.
		/// </summary>
		public static void Reset()
		{
			foreach (ISession session in Storage.GetAllSessions())
			{
				session.Dispose();
			}

			sessionFactories.Clear();

			Storage = null;
			RegisteredInterceptor = null;
			ValidatorEngine = null;
		}

		/// <summary>
		/// Return an ISessionFactory based on the specified factoryKey.
		/// </summary>
		/// <param name="factoryKey"></param>
		/// <returns></returns>
		public static ISessionFactory GetSessionFactoryFor(string factoryKey)
		{
			return sessionFactories[factoryKey];
		}

		/// <summary>
		/// Returns the default ISessionFactory using the DefaultFactoryKey.
		/// </summary>
		/// <returns></returns>
		public static ISessionFactory GetDefaultSessionFactory()
		{
			return GetSessionFactoryFor(DefaultFactoryKey);
		}

		#endregion

		#region Public Fields

		/// <summary>
		/// The default factory key used if only one database is being communicated with.
		/// </summary>
		public static readonly string DefaultFactoryKey = "nhibernate.current_session";

		#endregion

		#region Private Methods

		private static ISessionFactory CreateSessionFactoryFor(
			string[] mappingAssemblies,
			AutoPersistenceModel autoPersistenceModel,
			Configuration cfg,
			IPersistenceConfigurer persistenceConfigurer)
		{
			FluentConfiguration fluentConfiguration = Fluently.Configure(cfg);

			if (persistenceConfigurer != null)
			{
				fluentConfiguration.Database(persistenceConfigurer);
			}

			fluentConfiguration.Mappings(m =>
			{
				foreach (var mappingAssembly in mappingAssemblies)
				{
					var assembly = Assembly.LoadFrom(MakeLoadReadyAssemblyName(mappingAssembly));

					m.HbmMappings.AddFromAssembly(assembly);
					m.FluentMappings.AddFromAssembly(assembly)
						.ConventionDiscovery.AddAssembly(assembly);
				}

				if (autoPersistenceModel != null)
				{
					m.AutoMappings.Add(autoPersistenceModel);
				}
			});

			return fluentConfiguration.BuildSessionFactory();
		}

		private static string MakeLoadReadyAssemblyName(string assemblyName)
		{
			return (assemblyName.IndexOf(".dll") == -1)
				? assemblyName.Trim() + ".dll"
				: assemblyName.Trim();
		}

		private static Configuration ConfigureNHibernate(string cfgFile, IDictionary<string, string> cfgProperties)
		{
			Configuration cfg = new Configuration();

			if (cfgProperties != null)
				cfg.AddProperties(cfgProperties);

			if (string.IsNullOrEmpty(cfgFile))
				return cfg.Configure();

			return cfg.Configure(cfgFile);
		}

		private static void ConfigureNHibernateValidator(Configuration cfg, string validatorCfgFile)
		{
			ValidatorEngine engine = new ValidatorEngine();

			if (string.IsNullOrEmpty(validatorCfgFile))
				engine.Configure();
			else
				engine.Configure(validatorCfgFile);

			// Register validation listeners with the current NHib configuration
			ValidatorInitializer.Initialize(cfg, engine);

			ValidatorEngine = engine;
		}

		#endregion

		#region Private Fields

		private static IInterceptor RegisteredInterceptor;

		/// <summary>
		/// Maintains a dictionary of NHibernate session factories, one per database.  The key is 
		/// the "factory key" used to look up the associated database, and used to decorate respective
		/// repositories.  If only one database is being used, this dictionary contains a single
		/// factory with a key of <see cref="DefaultFactoryKey" />.
		/// </summary>
		private static Dictionary<string, ISessionFactory> sessionFactories = new Dictionary<string, ISessionFactory>();

		/// <summary>
		/// An application-specific implementation of ISessionStorage must be setup either thru
		/// <see cref="InitStorage" /> or one of the <see cref="Init" /> overloads. 
		/// </summary>
		private static ISessionStorage Storage { get; set; }

		#endregion
	}
}
