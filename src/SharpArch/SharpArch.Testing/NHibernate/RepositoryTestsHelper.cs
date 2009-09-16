using System;
using System.Configuration;
using System.Reflection;
using FluentNHibernate.Automapping;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using SharpArch.Core;
using SharpArch.Data.NHibernate;
using SharpArch.Data.NHibernate.FluentNHibernate;
using Configuration = NHibernate.Cfg.Configuration;
using System.Data;

namespace SharpArch.Testing.NHibernate
{
    /// <summary>
    /// Provides helper methods for consolidating duplicated code from test fixture base classes.
    /// </summary>
    public class RepositoryTestsHelper
    {
        public static void InitializeDatabase() {
            Configuration cfg = InitializeNHibernateSession();
            IDbConnection connection = NHibernateSession.Current.Connection;
            new SchemaExport(cfg).Execute(false, true, false, connection, null);
        }

        public static Configuration InitializeNHibernateSession() {
            string[] mappingAssemblies = GetMappingAssemblies();
            AutoPersistenceModel autoPersistenceModel = GetAutoPersistenceModel(mappingAssemblies);
            return NHibernateSession.Init(new SimpleSessionStorage(), mappingAssemblies, autoPersistenceModel);
        }

		public static void Shutdown()
		{
			NHibernateSession.CloseAllSessions();
			NHibernateSession.Reset();
		}

		public static void FlushSessionAndEvict(object instance)
		{
			// Commits any changes up to this point to the database
			NHibernateSession.Current.Flush();

			// Evicts the instance from the current session so that it can be loaded during testing;
			// this gives the test a clean slate, if you will, to work with
			NHibernateSession.Current.Evict(instance);
		}

        public static string[] GetMappingAssemblies() {
            string mappingAssembliesSetting = ConfigurationManager.AppSettings["nhibernate.mapping.assembly"];

            Check.Require(!string.IsNullOrEmpty(mappingAssembliesSetting),
                "Please add an AppSetting to your app.config for 'nhibernate.mapping.assembly.' This setting " +
                "takes a comma delimited list of assemblies containing NHibernate mapping files. Including '.dll' " +
                "at the end of each is optional.");

            return mappingAssembliesSetting.Split(',');
        }

        public static AutoPersistenceModel GetAutoPersistenceModel(string[] assemblies) {
            foreach (var asmName in assemblies) {
                Assembly asm = Assembly.Load(asmName);
                Type[] asmTypes = asm.GetTypes();

                foreach (Type asmType in asmTypes) {
                    if (typeof(IAutoPersistenceModelGenerator).IsAssignableFrom(asmType)) {
                        IAutoPersistenceModelGenerator generator = Activator.CreateInstance(asmType) as IAutoPersistenceModelGenerator;
                        return generator.Generate();
                    }
                }
            }

            return null;
        }
	}
}