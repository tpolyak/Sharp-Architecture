using System.Reflection;
using System.Collections.Generic;
using System;

namespace SharpArch.Data.NHibernate.FluentNHibernate
{
    /// <summary>
    /// An optional helper class used to view the mapping file generated from a fluent nhibernate class mapper
    /// </summary>
    /// <remarks>This is not necessary for Fluent Nhibernate to function properly.</remarks>
    public class GeneratorHelper
    {
        public static IList<IMapGenerator> GetMapGenerators() {
            IList<IMapGenerator> generators = new List<IMapGenerator>();
            Assembly assembly = Assembly.GetAssembly(typeof(IMapGenerator));

            foreach (Type type in assembly.GetTypes()) {
                if (null == type.GetInterface(GENERATOR_INTERFACE)) continue;
                var instance = Activator.CreateInstance(type) as IMapGenerator;

                if (instance != null)
                    generators.Add(instance);
            }

            return generators;
        }

        private const string GENERATOR_INTERFACE = "IMapGenerator";
    }
}
