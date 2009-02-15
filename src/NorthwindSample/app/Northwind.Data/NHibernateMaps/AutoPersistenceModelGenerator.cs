using System;
using System.Linq;
using FluentNHibernate;
using FluentNHibernate.AutoMap;
using Northwind.Core;
using Northwind.Data.NHibernateMappings;
using SharpArch.Core.DomainModel;
using SharpArch.Data.NHibernate.FluentNHibernate;
using Northwind.Data.NHibernateMappings.Organization;

namespace Northwind.Data.NHibernateMaps
{
    public class AutoPersistenceModelGenerator : IAutoPersistenceModelGenerator
    {
        public AutoPersistenceModel Generate() {
            AutoPersistenceModel mappings = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<Category>()
                .Where(GetAutoMappingFilter)
                .WithConvention(GetConventions)
                .UseOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();

            return mappings;
        }

        /// <summary>
        /// Provides a filter for only including types which inherit from the IEntityWithTypedId interface.
        /// This might be considered a little hackish having this magic string in the comparison, but since
        /// the interface is generic, it wouldn't be possible to compare the type directly.
        /// </summary>
        private bool GetAutoMappingFilter(Type t) {
            return t.GetInterfaces().Any(x =>
                 x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityWithTypedId<>));
        }

        private void GetConventions(Conventions c) {
            c.GetPrimaryKeyNameFromType = type => type.Name + "ID";
            c.FindIdentity = type => type.Name == "Id";
            c.GetTableName = type => Inflector.Net.Inflector.Pluralize(type.Name);
            c.IsBaseType = IsBaseTypeConvention;
            c.GetForeignKeyNameOfParent = type => type.Name + "ID";
        }

        private bool IsBaseTypeConvention(Type arg) {
            bool derivesFromEntity = arg == typeof(Entity);
            bool derivesFromEntityWithTypedId = arg.IsGenericType && 
                (arg.GetGenericTypeDefinition() == typeof(EntityWithTypedId<>));

            return derivesFromEntity || derivesFromEntityWithTypedId;
        }
    }
}
