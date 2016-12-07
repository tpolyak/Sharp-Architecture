namespace SharpArch.Web.Mvc.Castle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Domain.Reflection;
    using global::Castle.Windsor;
    using JetBrains.Annotations;
    using SharpArch.Castle.Extensions;

    /// <summary>
    ///     Filter provider which performs property dependency injection.
    /// </summary>
    /// <remarks>
    ///     Based on http://thirteendaysaweek.com/2012/09/17/dependency-injection-with-asp-net-mvc-action-filters/
    /// </remarks>
    [PublicAPI]
    public class WindsorFilterAttributeProvider : FilterAttributeFilterProvider
    {
        private readonly IWindsorContainer container;
        private readonly ITypePropertyDescriptorCache typePropertyDescriptorCache;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="container">Windsor container.</param>
        /// <param name="typePropertyDescriptorCache"></param>
        /// <exception cref="ArgumentNullException">Container is <c>null</c>.</exception>
        public WindsorFilterAttributeProvider([NotNull] IWindsorContainer container,
            [CanBeNull] ITypePropertyDescriptorCache typePropertyDescriptorCache) : base(false)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            this.container = container;
            this.typePropertyDescriptorCache = typePropertyDescriptorCache;
        }

        /// <summary>
        /// Aggregates the filters from all of the filter providers into one collection.
        /// </summary>
        /// <returns>
        /// The collection filters from all of the filter providers.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="actionDescriptor">The action descriptor.</param>
        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext,
            ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor).ToArray();

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < filters.Length; i++)
            {
                container.Kernel.InjectProperties(filters[i].Instance, typePropertyDescriptorCache);
            }

            return filters;
        }
    }
}
