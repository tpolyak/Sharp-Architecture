// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ForCanBeConvertedToForeach

namespace SharpArch.Web.Http.Castle
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Domain;
    using Domain.Reflection;
    using global::Castle.Core;
    using global::Castle.Windsor;
    using SharpArch.Castle.Extensions;

    /// <summary>
    ///     A Castle Windsor filter provider that supports property injection.
    /// </summary>
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
        public WindsorHttpFilterProvider(IWindsorContainer container,
            ITypePropertyDescriptorCache typePropertyDescriptorCache)
        {
            Check.Require(container != null);
            Check.Require(typePropertyDescriptorCache != null);

            this.container = container;
            this.typePropertyDescriptorCache = typePropertyDescriptorCache;
        }

        /// <summary>
        ///     Returns an enumeration of filters.
        /// </summary>
        /// <param name="configuration">The HTTP configuration.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>An enumeration of filters.</returns>
        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            Check.Require(configuration != null);
            Check.Require(actionDescriptor != null);

            List<FilterInfo> filters = new List<FilterInfo>();

            // Add global filters
            if (configuration.Filters.Count > 0)
            {
                filters.AddRange(configuration.Filters);
            }

            // Add controller filters
            var controllerFilters = actionDescriptor.ControllerDescriptor.GetFilters();
            for (int i = 0; i < controllerFilters.Count; i++)
            {
                filters.Add(new FilterInfo(controllerFilters[i], FilterScope.Controller));
            }

            // Add action filters
            var actionFilters = actionDescriptor.GetFilters();
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
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "Dependency '{0}' with lifestyle {1} can not be injected into property {2}. Since  WebAPI Filters are singletons, either use service location or make dependency a Singleton.",
                    propertyInfo.PropertyType.FullName, dependencyRegistration.LifestyleType, propertyInfo.Name
                    ))
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