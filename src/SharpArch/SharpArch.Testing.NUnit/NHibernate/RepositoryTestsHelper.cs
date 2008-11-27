using SharpArch.Core;
using NHibernate.Cfg;
using System.Data;
using NHibernate.Tool.hbm2ddl;
using SharpArch.Data.NHibernate;
using System.Configuration;
namespace SharpArch.Testing.NUnit.NHibernate
{
    /// <summary>
    /// Provides helper methods for consolidating duplicated code from test fixture base classes.
    /// </summary>
    public class RepositoryTestsHelper
    {
        public static void InitializeDatabase() {
            string[] mappingAssemblies = GetMappingAssemblies();
            
            Configuration cfg = NHibernateSession.Init(new SimpleSessionStorage(), mappingAssemblies);
            IDbConnection connection = NHibernateSession.Current.Connection;
            new SchemaExport(cfg).Execute(false, true, false, true, connection, null);
        }

        public static string[] GetMappingAssemblies() {
            string mappingAssembliesSetting = ConfigurationSettings.AppSettings["nhibernate.mapping.assembly"];

            Check.Require(!string.IsNullOrEmpty(mappingAssembliesSetting),
                "Please add an AppSetting to your app.config for 'nhibernate.mapping.assembly.' This setting " +
                "takes a comma delimited list of assemblies containing NHibernate mapping files. Including '.dll' " +
                "at the end of each is optional.");

            return mappingAssembliesSetting.Split(',');
        }
    }
}
