// ReSharper disable CheckNamespace

// This file is shared between Sharp.Web.Http.Castle and Sharp.Web.Mvc.Castle

namespace SharpArch.Castle.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using global::Castle.Core;
    using global::Castle.MicroKernel;
    using global::Castle.MicroKernel.ComponentActivator;
    using SharpArch.Domain.Reflection;

    internal static class WindsorPropertyInjectionExtensions
    {
        /// <summary>
        ///     Injects dependencies into properties.
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="target">The target object to inject properties.</param>
        /// <param name="cache">Injectable property descriptor cache.</param>
        /// <param name="validatePropertyRegistration">
        ///     Callback to validate property dependency to be injected. Must throw
        ///     exception in case of failure.
        /// </param>
        /// <exception cref="ComponentActivatorException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="target" /> is <see langword="null" />.</exception>
        public static void InjectProperties(this IKernel kernel, object target, ITypePropertyDescriptorCache cache,
            Action<PropertyInfo, ComponentModel> validatePropertyRegistration = null)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            Type type = target.GetType();

            // Cache miss expected only once per given type, so call Find() first to prevent extra closure allocation in GetOrAdd.
            TypePropertyDescriptor info = cache != null
                ? cache.Find(type) ?? GetOrAdd(kernel, cache, type)
                : GetInjectableProperties(type, kernel);


            if (!info.HasProperties())
            {
                return;
            }

            for (var i = 0; i < info.Properties.Length; i++)
            {
                PropertyInfo injectableProperty = info.Properties[i];
                if (validatePropertyRegistration != null)
                {
                    IHandler registration = kernel.GetHandler(injectableProperty.PropertyType);
                    if (registration != null)
                    {
                        validatePropertyRegistration(injectableProperty, registration.ComponentModel);
                    }
                }
                object val = kernel.Resolve(injectableProperty.PropertyType);
                try
                {
                    injectableProperty.SetValue(target, val, null);
                }

                catch (Exception ex)
                {
                    string message = string.Format(
                        "Error injecting property {0} on type {1}, See inner exception for more information.",
                        injectableProperty.Name, type.FullName);

                    throw new ComponentActivatorException(message, ex, null);
                }
            }
        }

        static TypePropertyDescriptor GetOrAdd(IKernel kernel, ITypePropertyDescriptorCache cache, Type type)
        {
            return cache.GetOrAdd(type, t => GetInjectableProperties(t, kernel));
        }

        static TypePropertyDescriptor GetInjectableProperties(Type type, IKernel kernel)
        {
            var injectableProperties = new List<PropertyInfo>();
            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.CanWrite && !prop.PropertyType.IsValueType && kernel.HasComponent(prop.PropertyType))
                {
                    injectableProperties.Add(prop);
                }
            }

            return new TypePropertyDescriptor(type,
                injectableProperties.Count > 0 ? injectableProperties.ToArray() : null);
        }
    }
}
