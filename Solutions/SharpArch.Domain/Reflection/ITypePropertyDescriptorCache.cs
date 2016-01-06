namespace SharpArch.Domain.Reflection
{
    using System;
    using DomainModel;

    /// <summary>
    ///     Property descriptors cache.
    /// </summary>
    /// <remarks>Implementation is thread-safe.
    /// todo: update <see cref="BaseObject"/> to use cache.
    /// </remarks>
    public interface ITypePropertyDescriptorCache
    {
        /// <summary>
        /// Find cached property descriptor.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><see cref="TypePropertyDescriptor"/> or <c>null</c> if does not exists.</returns>
        TypePropertyDescriptor Find(Type type);

        /// <summary>
        /// Get existing property descriptor or create and cache it.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="factory">The factory to create descriptor.</param>
        /// <returns></returns>
        TypePropertyDescriptor GetOrAdd(Type type, Func<Type, TypePropertyDescriptor> factory);

        /// <summary>
        ///     Clears the cache.
        /// </summary>
        void Clear();
    }
}