// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ForCanBeConvertedToForeach

namespace SharpArch.Web.Http.Castle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Domain.Reflection;
    using global::Castle.Core;
    using global::Castle.Windsor;
    using JetBrains.Annotations;
    using SharpArch.Castle.Extensions;

    /// <summary>
    ///     A Castle Windsor filter provider that supports property injection.
    /// </summary>
    [PublicAPI]
    public class WindsorHttpFilterProvider : IFilterProvider
    {
        /// <summary>
        ///     The container
        /// </summary>
        private readonly IWindsorContainer container;

        private readonly ITypePropertyDescriptorCache typePropertyDescriptorCache;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WindsorHttpFilterProvider" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="typePropertyDescriptorCache">Property descriptor cache.</param>
        public WindsorHttpFilterProvider([NotNull] IWindsorContainer container,
            [NotNull] ITypePropertyDescriptorCache typePropertyDescriptorCache)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (typePropertyDescriptorCache == null)
                throw new ArgumentNullException(nameof(typePropertyDescriptorCache));

            this.container = container;
            this.typePropertyDescriptorCache = typePropertyDescriptorCache;
        }

        /// <summary>
        ///     Returns an enumeration of filters.
        /// </summary>
        /// <param name="configuration">The HTTP configuration.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>An enumeration of filters.</returns>
        [NotNull]
        public IEnumerable<FilterInfo> GetFilters([NotNull] HttpConfiguration configuration,
            [NotNull] HttpActionDescriptor actionDescriptor)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (actionDescriptor == null) throw new ArgumentNullException(nameof(actionDescriptor));

            var controllerFilters = actionDescriptor.ControllerDescriptor.GetFilters();
            var actionFilters = actionDescriptor.GetFilters();
            int totalFilterCount = configuration.Filters.Count + controllerFilters.Count + actionFilters.Count;

            if (totalFilterCount == 0)
               return Enumerable.Empty<FilterInfo>();


            List<FilterInfo> filters = new List<FilterInfo>(totalFilterCount);

            // Add global filters
            if (configuration.Filters.Count > 0)
            {
                filters.AddRange(configuration.Filters);
            }

            // Add controller filters
            for (int i = 0; i < controllerFilters.Count; i++)
            {
                filters.Add(new FilterInfo(controllerFilters[i], FilterScope.Controller));
            }

            // Add action filters
            for (int i = 0; i < actionFilters.Count; i++)
            {
                filters.Add(new FilterInfo(actionFilters[i], FilterScope.Action));
            }


            for (int i = 0; i < filters.Count; i++)
            {
                // ReSharper disable once ConvertClosureToMethodGroup
                this.container.Kernel.InjectProperties(filters[i].Instance, typePropertyDescriptorCache,
                    (info, model) => ValidateFilterDependency(info, model));
            }

            return filters;
        }

        private static void ValidateFilterDependency(PropertyInfo propertyInfo, ComponentModel dependencyRegistration)
        {
            if (dependencyRegistration.LifestyleType != LifestyleType.Singleton)
            {
                throw new InvalidOperationException(
                    $"Dependency '{propertyInfo.PropertyType.FullName}' with lifestyle {dependencyRegistration.LifestyleType} can not be injected into property {propertyInfo.Name}. Since  WebAPI Filters are singletons, either use service location or make dependency a Singleton.")
                {
                    Data =
                    {
                        {
                            "Additional information",
                            "http://docs.autofac.org/en/latest/integration/webapi.html#standard-web-api-filters-are-singletons"
                        }
                    }
                };
            }
        }
    }
}