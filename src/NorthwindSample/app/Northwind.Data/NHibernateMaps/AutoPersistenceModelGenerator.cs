using FluentNHibernate;
using FluentNHibernate.AutoMap;
using Northwind.Core;
using SharpArch.Core.DomainModel;
using SharpArch.Data.NHibernate.FluentNHibernate;
using System;

namespace Northwind.Data.NHibernateMaps
{
    public class AutoPersistenceModelGenerator : IAutoPersistenceModelGenerator
    {
        public AutoPersistenceModel Generate() {
            AutoPersistenceModel mappings = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<Category>()
                .Where(GetAutoMappingFilter)
                .WithConvention(GetConventions);
            return mappings;
        }


        private bool GetAutoMappingFilter(Type t) {
            return t.Namespace == "Northwind.Core" &&
                // NHibernate persistence auto-mapping doesn't play nice with extension methods, so exclude them
                (t.Name.Contains("OrdersExtensions") == false);
        }

        private void GetConventions(Conventions c) {
            c.GetPrimaryKeyNameFromType = type => type.Name + "ID";
            c.FindIdentity = type => type.Name == "ID";
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
