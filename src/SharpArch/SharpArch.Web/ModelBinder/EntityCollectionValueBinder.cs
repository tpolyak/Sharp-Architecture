using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using SharpArch.Core.DomainModel;

namespace SharpArch.Web.ModelBinder
{
    class EntityCollectionValueBinder : DefaultModelBinder
    {
        #region Implementation of IModelBinder

        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <returns>
        /// The bound value.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="bindingContext">The binding context.</param>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Type collectionType = bindingContext.ModelType;
            Type collectionEntityType = collectionType.GetGenericArguments().First();

            bool containsPrefix = bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName);
            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult != null)
            {
                int countOfEntityIds = (valueProviderResult.RawValue as string[]).Length;
                Array entities = Array.CreateInstance(collectionEntityType, countOfEntityIds);

                Type entityInterfaceType = collectionEntityType.GetInterfaces()
                    .First(interfaceType => interfaceType.IsGenericType
                                            && interfaceType.GetGenericTypeDefinition() == typeof(IEntityWithTypedId<>));

                Type idType = entityInterfaceType.GetGenericArguments().First();

                for ( int i = 0; i < countOfEntityIds; i++ )
                {
                    string rawId = (valueProviderResult.RawValue as string[])[i];

                    if ( string.IsNullOrEmpty(rawId) )
                        return null;

                    object typedId = 
                        (idType == typeof(Guid))
                            ? new Guid(rawId)
                            : Convert.ChangeType(rawId, idType);

                    object entity = GetEntityFor(collectionEntityType, typedId, idType);
                    entities.SetValue(entity, i);
                }

                //base.BindModel(controllerContext, bindingContext);
                return entities;
            }
            return base.BindModel(controllerContext, bindingContext);
        }

        #endregion

        
        protected static object GetEntityFor(Type collectionEntityType, object typedId, Type idType)
        {
            object entityRepository = GenericRepositoryFactory.CreateEntityRepositoryFor(collectionEntityType, idType);

            return entityRepository.GetType()
                .InvokeMember("Get", BindingFlags.InvokeMethod, null, entityRepository, new[] { typedId });
        }
    }
}
