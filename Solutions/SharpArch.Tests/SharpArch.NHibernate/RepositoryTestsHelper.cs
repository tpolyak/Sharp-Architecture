namespace Tests.SharpArch.NHibernate
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;

    using FluentNHibernate.Automapping;
    using global::SharpArch.Domain;
    using global::SharpArch.NHibernate.FluentNHibernate;

    /// <summary>
    ///     Provides helper methods for consolidating duplicated code from test fixture base classes.
    /// </summary>
    public class RepositoryTestsHelper
    {
        [CLSCompliant(false)]
        public static AutoPersistenceModel GetAutoPersistenceModel(string[] assemblies)
        {
            return (from asmName in assemblies
                    select Assembly.Load(asmName)
                    into asm 
                    from asmType in asm.GetTypes()
                    where typeof(IAutoPersistenceModelGenerator).IsAssignableFrom(asmType)
                    select Activator.CreateInstance(asmType) as IAutoPersistenceModelGenerator
                    into generator 
                    select generator.Generate()).FirstOrDefault();
        }

        public static string[] GetMappingAssemblies()
        {
            var mappingAssembliesSetting = ConfigurationManager.AppSettings["nhibernate.mapping.assembly"];

            Check.Require(
                !string.IsNullOrEmpty(mappingAssembliesSetting), 
                "Please add an AppSetting to your app.config for 'nhibernate.mapping.assembly.' This setting " +
                "takes a comma delimited list of assemblies containing NHibernate mapping files. Including '.dll' " +
                "at the end of each is optional.");

            return mappingAssembliesSetting.Split(',');
        }
    }
}