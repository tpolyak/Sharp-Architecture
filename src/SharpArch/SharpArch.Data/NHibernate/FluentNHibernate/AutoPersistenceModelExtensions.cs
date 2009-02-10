using FluentNHibernate.AutoMap;
using System.Reflection;
using System;
using SharpArch.Core;

namespace SharpArch.Data.NHibernate.FluentNHibernate
{
    /// <summary>
    /// Provides a means to override <see cref="AutoPersistenceModel" /> conventions with classes 
    /// that implement <see cref="IAutoPeristenceModelConventionOverride" />.
    /// </summary>
    public static class AutoPersistenceModelExtensions
    {
        public static AutoPersistenceModel MapConventionOverridesFromAssemblyOf<T>(
            this AutoPersistenceModel autoPersistenceModel) {

            Assembly assemblyToPullConventionOverridesFrom = typeof(T).Assembly;

            foreach (Type type in assemblyToPullConventionOverridesFrom.GetTypes()) {
                if (typeof(IAutoPeristenceModelConventionOverride).IsAssignableFrom(type)) {
                    IAutoPeristenceModelConventionOverride instance =
                        Activator.CreateInstance(type) as IAutoPeristenceModelConventionOverride;

                    if (instance != null)
                        instance.Override(autoPersistenceModel);
                }
            }

            return autoPersistenceModel;
        }
    }
}
