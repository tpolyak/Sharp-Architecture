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
            cache.TryGetValue(type, out result);
            return result;
        }

        public TypePropertyDescriptor GetOrAdd(Type type, Func<TypePropertyDescriptor> factory)
        {
            return cache.GetOrAdd(type, factory());
        }

        public void Clear()
        {
            cache.Clear();
        }
    }
}