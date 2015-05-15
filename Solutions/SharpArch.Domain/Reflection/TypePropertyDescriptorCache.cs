namespace SharpArch.Domain.Reflection
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    ///     Property descriptors cache.
    /// </summary>
    /// <remarks>Implementation is thread-safe.</remarks>
    public class TypePropertyDescriptorCache : ITypePropertyDescriptorCache
    {
        private readonly ConcurrentDictionary<Type, TypePropertyDescriptor> cache =
            new ConcurrentDictionary<Type, TypePropertyDescriptor>();

        public TypePropertyDescriptor Find(Type type)
        {
            TypePropertyDescriptor result;
            this.cache.TryGetValue(type, out result);
            return result;
        }

        public TypePropertyDescriptor GetOrAdd(Type type, Func<TypePropertyDescriptor> factory)
        {
            Check.Require(factory != null, "Value factory can not be null.");
            return this.cache.GetOrAdd(type, factory());
        }

        public void Clear()
        {
            this.cache.Clear();
        }
    }
}
