namespace SharpArch.Web.Mvc.ModelBinder
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using SharpArch.Domain.DomainModel;

    internal class EntityValueBinder : SharpModelBinder
    {
        /// <summary>
        ///     Binds the model value to an entity by using the specified controller context and binding context.
        /// </summary>
        /// <returns>
        ///     The bound value.
        /// </returns>
        /// <param name = "controllerContext">The controller context.</param>
        /// <param name = "bindingContext">The binding context.</param>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var modelType = bindingContext.ModelType;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult != null)
            {
                var entityInterfaceType =
                    modelType.GetInterfaces().First(
                        interfaceType =>
                        interfaceType.IsGenericType &&
                        interfaceType.GetGenericTypeDefinition() == typeof(IEntityWithTypedId<>));

                var idType = entityInterfaceType.GetGenericArguments().First();
                var rawId = (valueProviderResult.RawValue as string[]).First();

                if (string.IsNullOrEmpty(rawId))
                {
                    return null;
                }

                try
                {
                    var typedId = (idType == typeof(Guid)) ? new Guid(rawId) : Convert.ChangeType(rawId, idType);

                    return ValueBinderHelper.GetEntityFor(modelType, typedId, idType);
                }
                catch (Exception)
                {
                    // If the Id conversion failed for any reason, just return null
                    return null;
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}