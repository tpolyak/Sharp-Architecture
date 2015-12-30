// ReSharper disable CheckNamespace

// This file is shared between Sharp.Web.Http.Castle and Sharp.Web.Mvc.Castle

namespace SharpArch.Castle.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Domain.Reflection;
    using global::Castle.Core;
    using global::Castle.MicroKernel;
    using global::Castle.MicroKernel.ComponentActivator;

    internal static class WindsorPropertyInjectionExtensions
    {
        /// <summary>
        ///     Injects dependencies into properties.
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="target">The target object to inject properties.</param>
        /// <param name="cache">Injectable property descriptor cache.</param>
        /// <param name="validatePropertyRegistration">Callback to validate property dependency to be injected. Must throw exception in case of failure.</param>
        /// <exception cref="ComponentActivatorException"></exception>
        public static void InjectProperties(this IKernel kernel, object target, ITypePropertyDescriptorCache cache, Action<PropertyInfo, ComponentModel> validatePropertyRegistration = null)
        {
            if (target == null) throw new ArgumentNullException("target");

            var type = target.GetType();

            var info = cache != null
                ? cache.Find(type) ?? cache.GetOrAdd(type, () => GetInjectableProperties(type, kernel))
                : GetInjectableProperties(type, kernel);


            if (!info.HasProperties()) return;

            for (int i = 0; i < info.Properties.Length; i++)
            {
                var injectableProperty = info.Properties[i];
                if (validatePropertyRegistration != null)
                {
                    var registration = kernel.GetHandler(injectableProperty.PropertyType);
                    if (registration != null)
                    {
                        validatePropertyRegistration(injectableProperty, registration.ComponentModel);
                    }
                }
                var val = kernel.Resolve(injectableProperty.PropertyType);
                try
                {
                    injectableProperty.SetValue(target, val, null);
                }

                catch (Exception ex)
                {
                    var message = string.Format(
                        "Error setting property {0} on type {1}, See inner exception for more information.",
                        injectableProperty.Name, type.FullName);

                    throw new ComponentActivatorException(message, ex, null);
                }
            }
        }

        private static TypePropertyDescriptor GetInjectableProperties(Type type, IKernel kernel)
        {
            var injectableProperties = (from prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where prop.CanWrite
                      && !prop.PropertyType.IsValueType
                      && kernel.HasComponent(prop.PropertyType)
                select prop
                ).ToArray();
            return new TypePropertyDescriptor(type,
                injectableProperties.Any() ? injectableProperties : null);
        }
    }
}