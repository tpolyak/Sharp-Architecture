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
                .UseOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();

            return mappings;
        }

        /// <summary>
        /// Provides a filter for only including types which inherit from the IEntityWithTypedId interface.
        /// </summary>
        private bool GetAutoMappingFilter(Type t) {
            return t.GetInterfaces().Any(x =>
                 x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityWithTypedId<>));
        }

        /// <summary>
        /// This project's conventions (which may be changed) are as follows:
        /// * Table names are plural
        /// * The Id of an object is "Id"
        /// * Foreign keys are "ObjectNameFk"
        /// * One-to-Many relationships cascade "All"
        /// 
        /// Feel free to change this to your project's needs!
        /// </summary>
        private void GetConventions(Conventions c) {
            c.GetTableName = type => Inflector.Net.Inflector.Pluralize(type.Name);
            c.IsBaseType = IsBaseTypeConvention;
            c.FindIdentity = type => type.Name == "Id";
            c.GetForeignKeyName = type => type.Name + "Fk";
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
