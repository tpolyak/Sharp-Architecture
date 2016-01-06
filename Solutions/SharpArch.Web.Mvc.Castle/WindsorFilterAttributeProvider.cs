namespace SharpArch.Web.Mvc.Castle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Domain.Reflection;
    using global::Castle.Windsor;
    using SharpArch.Castle.Extensions;

    /// <summary>
    ///     Filter provider which performs property dependency injection.
    /// </summary>
    /// <remarks>
    ///     Based on http://thirteendaysaweek.com/2012/09/17/dependency-injection-with-asp-net-mvc-action-filters/
    /// </remarks>
    public class WindsorFilterAttributeProvider : FilterAttributeFilterProvider
    {
        private readonly IWindsorContainer container;
        private readonly ITypePropertyDescriptorCache typePropertyDescriptorCache;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="container">Windsor container.</param>
        /// <param name="typePropertyDescriptorCache"></param>
        /// <exception cref="ArgumentNullException">Container is null.</exception>
        public WindsorFilterAttributeProvider(IWindsorContainer container, ITypePropertyDescriptorCache typePropertyDescriptorCache) : base(false)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.container = container;
            this.typePropertyDescriptorCache = typePropertyDescriptorCache;
        }

        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext,
            ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor).ToArray();
            for (var i = 0; i < filters.Length; i++)
            {
                container.Kernel.InjectProperties(filters[i].Instance, typePropertyDescriptorCache);
            }

            return filters;
        }
    }
}
