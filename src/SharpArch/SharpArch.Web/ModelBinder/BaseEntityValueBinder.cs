using System;
using System.Reflection;
using System.Web.Mvc;

namespace SharpArch.Web.ModelBinder
{
    abstract class BaseEntityValueBinder : IModelBinder
    {
        #region Implementation of IModelBinder

        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <returns>
        /// The bound value.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="bindingContext">The binding context.</param>
        public abstract object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext);

        #endregion

        protected static object GetEntityFor(Type collectionEntityType, object typedId, Type idType)
        {
            object entityRepository = GenericRepositoryFactory.CreateEntityRepositoryFor(collectionEntityType, idType);

            return entityRepository.GetType()
                .InvokeMember("Get", BindingFlags.InvokeMethod, null, entityRepository, new[] { typedId });
        }
    }
}
