using System.Web.Mvc;
using System.ComponentModel;
using System;
using SharpArch.Core.CommonValidator;
using SharpArch.Core.DomainModel;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using System.Collections;
using SharpArch.Web.CommonValidator;

namespace SharpArch.Web.ModelBinder
{
    public class SharpModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// Called when the model is updating. We handle updating the Id property here because it gets filtered out
        /// of the normal MVC2 property binding.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>
        /// true if the model is updating; otherwise, false.
        /// </returns>
        protected override bool OnModelUpdating(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if ( IsEntityType(bindingContext.ModelType) )
            {
                //handle the Id property
                PropertyDescriptor idProperty =
                    (from PropertyDescriptor property in TypeDescriptor.GetProperties(bindingContext.ModelType)
                     where property.Name == ID_PROPERTY_NAME
                     select property).SingleOrDefault();

                BindProperty(controllerContext, bindingContext, idProperty);

            }
            return base.OnModelUpdating(controllerContext, bindingContext);
        }

        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            Type propertyType = propertyDescriptor.PropertyType;

            if ( IsEntityType(propertyType) )
            {
                //use the EntityValueBinder
                return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, new EntityValueBinder());
            }
            
            if ( IsSimpleGenericBindableEntityCollection(propertyType) )
            {
                //use the EntityValueCollectionBinder
                return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, new EntityCollectionValueBinder());
            }

            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }

        protected override void SetProperty(ControllerContext controllerContext,
            ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            if ( propertyDescriptor.Name == ID_PROPERTY_NAME )
            {
                SetIdProperty(bindingContext, propertyDescriptor, value);
            }
            else if (value as IEnumerable != null && IsSimpleGenericBindableEntityCollection(propertyDescriptor.PropertyType))
            {
                SetEntityCollectionProperty(bindingContext, propertyDescriptor, value);
            }
            else
            {
                base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
            }

        }

        private static bool IsEntityType(Type propertyType)
        {
            bool isEntityType = propertyType.GetInterfaces()
                .Any(type => type.IsGenericType &&
                    type.GetGenericTypeDefinition() == typeof(IEntityWithTypedId<>));

            return isEntityType;
        }

        private static bool IsSimpleGenericBindableEntityCollection(Type propertyType)
        {
            bool isSimpleGenericBindableCollection =
                propertyType.IsGenericType &&
                (propertyType.GetGenericTypeDefinition() == typeof(IList<>) ||
                 propertyType.GetGenericTypeDefinition() == typeof(ICollection<>) ||
                 propertyType.GetGenericTypeDefinition() == typeof(ISet<>) ||
                 propertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            bool isSimpleGenericBindableEntityCollection =
                isSimpleGenericBindableCollection && IsEntityType(propertyType.GetGenericArguments().First());

            return isSimpleGenericBindableEntityCollection;
        }

        /// <summary>
        /// If the property being bound is an Id property, then use reflection to get past the 
        /// protected visibility of the Id property, accordingly.
        /// </summary>
        private static void SetIdProperty(ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor, object value)
        {
            Type idType = propertyDescriptor.PropertyType;
            object typedId = Convert.ChangeType(value, idType);

            // First, look to see if there's an Id property declared on the entity itself; 
            // e.g., using the new keyword
            PropertyInfo idProperty = bindingContext.ModelType
                .GetProperty(propertyDescriptor.Name,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            // If an Id property wasn't found on the entity, then grab the Id property from
            // the entity base class
            if ( idProperty == null )
            {
                idProperty = bindingContext.ModelType
                    .GetProperty(propertyDescriptor.Name,
                        BindingFlags.Public | BindingFlags.Instance);
            }

            // Set the value of the protected Id property
            idProperty.SetValue(bindingContext.Model, typedId, null);
        }


        /// <summary>
        /// If the property being bound is a simple, generic collection of entiy objects, then use 
        /// reflection to get past the protected visibility of the collection property, if necessary.
        /// </summary>
        private static void SetEntityCollectionProperty(ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor, object value)
        {
            object entityCollection = propertyDescriptor.GetValue(bindingContext.Model);
            if (entityCollection != value)
            {
                Type entityCollectionType = entityCollection.GetType();

                foreach (object entity in (value as IEnumerable))
                {
                    entityCollectionType.InvokeMember("Add",
                                                      BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, entityCollection,
                                                      new object[] { entity });
                }
            }
        }

        #region Overridable (but not yet overridden) Methods

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return base.BindModel(controllerContext, bindingContext);
        }

        protected override object CreateModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext, Type modelType)
        {

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }

        protected override void BindProperty(ControllerContext controllerContext,
    ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }

        protected override void OnPropertyValidated(ControllerContext controllerContext,
            ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            base.OnPropertyValidated(controllerContext, bindingContext, propertyDescriptor, value);
        }

        protected override bool OnPropertyValidating(ControllerContext controllerContext,
            ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {

            return base.OnPropertyValidating(controllerContext, bindingContext, propertyDescriptor, value);
        }

        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.OnModelUpdated(controllerContext, bindingContext);
        }
        #endregion

        private const string ID_PROPERTY_NAME = "Id";
    }
}
