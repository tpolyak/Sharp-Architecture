// ReSharper disable CheckNamespace

// This file is shared between Sharp.Web.Http.Castle and Sharp.Web.Mvc.Castle

namespace SharpArch.Castle.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using global::Castle.Core;
    using global::Castle.MicroKernel;
    using global::Castle.MicroKernel.ComponentActivator;
    using JetBrains.Annotations;
    using SharpArch.Domain.Reflection;

    /// <summary>
    ///     Property injection support for Windsor.
    /// </summary>
    internal static class WindsorPropertyInjectionExtensions
    {

        /// <summary>
        ///     Injects dependencies into properties.
        /// </summary>
        /// <param name="kernel">Windsor kernel</param>
        /// <param name="target">The target object to inject properties.</param>
        /// <param name="cache">Injectable property descriptor cache.</param>
        /// <param name="validatePropertyRegistration">
        ///     Callback to validate property dependency to be injected. Must throw
        ///     exception in case of failure.
        /// </param>
        /// <exception cref="ComponentActivatorException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="target" /> is <see langword="null" />.</exception>
        public static void InjectProperties([NotNull] this IKernel kernel, [NotNull] object target,
            [CanBeNull] ITypePropertyDescriptorCache cache,
            Action<PropertyInfo, ComponentModel> validatePropertyRegistration = null)
        {
            if (kernel == null)
                throw new ArgumentNullException(nameof(kernel));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            Type type = target.GetType();
            TypePropertyDescriptor info = GetInjectableProperties(kernel, cache, validatePropertyRegistration, type);

            if (!info.HasProperties())
                return;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < info.Properties.Length; i++)
            {
                PropertyInfo injectableProperty = info.Properties[i];
                object val = kernel.Resolve(injectableProperty.PropertyType);
                try
                {
                    injectableProperty.SetValue(target, val, null);
                }
                catch (Exception ex)
                {
                    string message =
                        $"Error injecting property {injectableProperty.Name} on type {type.FullName}, See inner exception for more information.";
                    throw new ComponentActivatorException(message, ex, null);
                }
            }
        }


        /// <summary>
        ///     Cleanups up injectable properties. All injectable properties will be set to <c>null</c>.
        /// </summary>
        /// <remarks>
        ///     Since this method does not perform dependency validation
        ///     <see cref="InjectProperties" /> , <paramref name="cache" /> is used as read-only.
        ///     Calling <see cref="CleanupInjectableProperties" /> without call to <see cref="InjectProperties" /> on the same type
        ///     will incur performance penalty as list of properties will be evaluated every time.
        /// </remarks>
        /// <param name="kernel">Windsor kernel.</param>
        /// <param name="target">The target object to cleanup injectable properties.</param>
        /// <param name="cache">Injectable property descriptor cache.</param>
        public static void CleanupInjectableProperties([NotNull] this IKernel kernel, [NotNull] object target,
            ITypePropertyDescriptorCache cache)
        {
            if (kernel == null)
                throw new ArgumentNullException(nameof(kernel));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            Type type = target.GetType();
            // Cache miss expected only once per given type, so call Find() first to prevent extra closure allocation in GetOrAdd.
            TypePropertyDescriptor info = cache?.Find(type) ?? GetInjectableProperties(type, kernel, null);

            if (!info.HasProperties())
                return;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < info.Properties.Length; i++)
            {
                info.Properties[i].SetValue(target, null, null);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TypePropertyDescriptor GetInjectableProperties(IKernel kernel, ITypePropertyDescriptorCache cache,
            Action<PropertyInfo, ComponentModel> validatePropertyRegistration, Type type)
        {
            // Cache miss expected only once per given type, so call Find() first to prevent extra closure allocation in GetOrAdd.
            TypePropertyDescriptor info = cache != null
                ? cache.Find(type) ?? GetOrAdd(kernel, cache, type, validatePropertyRegistration)
                : GetInjectableProperties(type, kernel, validatePropertyRegistration);
            return info;
        }

        private static TypePropertyDescriptor GetOrAdd(IKernel kernel, ITypePropertyDescriptorCache cache, Type type,
            Action<PropertyInfo, ComponentModel> validatePropertyRegistration)
        {
            return cache.GetOrAdd(type, t => GetInjectableProperties(t, kernel, validatePropertyRegistration));
        }

        private static TypePropertyDescriptor GetInjectableProperties(Type type, IKernel kernel,
            Action<PropertyInfo, ComponentModel> validatePropertyRegistration)
        {
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (properties.Length == 0)
                return null;

            var injectableProperties = new List<PropertyInfo>(properties.Length);
            foreach (PropertyInfo prop in properties)
            {
                if (!prop.CanWrite || prop.PropertyType.IsValueType)
                    continue;
                IHandler registration = kernel.GetHandler(prop.PropertyType);
                if (registration != null)
                {
                    validatePropertyRegistration?.Invoke(prop, registration.ComponentModel);
                    injectableProperties.Add(prop);
                }
            }

            return new TypePropertyDescriptor(type,
                injectableProperties.Count > 0 ? injectableProperties.ToArray() : null);
        }
    }
}
