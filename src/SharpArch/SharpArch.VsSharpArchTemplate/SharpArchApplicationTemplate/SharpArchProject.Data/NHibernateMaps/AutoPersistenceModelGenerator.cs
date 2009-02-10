using System;
using System.Linq;
using FluentNHibernate;
using FluentNHibernate.AutoMap;
using SharpArch.Core.DomainModel;
using SharpArch.Data.NHibernate.FluentNHibernate;
using $solutionname$.Core;

namespace $safeprojectname$.NHibernateMaps
{
    public class AutoPersistenceModelGenerator : IAutoPersistenceModelGenerator
    {
        public AutoPersistenceModel Generate() {
            AutoPersistenceModel mappings = AutoPersistenceModel
                // If you delete the default class, simply point the following line to an entity within the .Core layer
                .MapEntitiesFromAssemblyOf<Class1>()
                .Where(GetAutoMappingFilter)
                .WithConvention(GetConventions)
                .MapConventionOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();

            return mappings;
        }

        /// <summary>
        /// Provides a filter for only including types which inherit from the IEntityWithTypedId interface.
        /// This might be considered a little hackish having this magic string in the comparison, but since
        /// the interface is generic, it wouldn't be possible to compare the type directly.
        /// </summary>
        private bool GetAutoMappingFilter(Type t) {
            return
                (from interfaceType in t.GetInterfaces()
                 where interfaceType.ToString().IndexOf("SharpArch.Core.DomainModel.IEntityWithTypedId") == 0
                 select interfaceType).Any();
        }

        /// <summary>
        /// This project's conventions (which may be changed) are as follows:
        /// * Table names are plural
        /// * The Id of an object is "Id"
        /// * Foreign keys are "ObjectNameFk"
        /// * One-to-Many relationships cascade "All"
        /// </summary>
        private void GetConventions(Conventions c) {
            c.GetTableName = type => Inflector.Net.Inflector.Pluralize(type.Name);
            c.IsBaseType = IsBaseTypeConvention;
            c.FindIdentity = type => type.Name == "ID";
            c.GetForeignKeyNameOfParent = type => type.Name + "Fk";
            c.OneToManyConvention = o => o.Cascade.All();
        }

        private bool IsBaseTypeConvention(Type arg) {
            bool derivesFromEntity = arg == typeof(Entity);
            bool derivesFromEntityWithTypedId = arg.IsGenericType && 
                (arg.GetGenericTypeDefinition() == typeof(EntityWithTypedId<>));
            return derivesFromEntity || derivesFromEntityWithTypedId;
        }
    }
}
