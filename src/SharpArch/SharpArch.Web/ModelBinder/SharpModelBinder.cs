using System.Web.Mvc;
using System.ComponentModel;
using SharpArch.Core.CommonValidator;
using SharpArch.Web.CommonValidator;
using System;
using SharpArch.Core.DomainModel;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using System.Collections;

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
            //handle the Id property
            PropertyDescriptor idProperty =
                (from PropertyDescriptor property in TypeDescriptor.GetProperties(bindingContext.ModelType)
                 where property.Name == ID_PROPERTY_NAME
                 select property).SingleOrDefault();

            if ( idProperty != null )
            {
                BindProperty(controllerContext, bindingContext, idProperty);
            }

            return base.OnModelUpdating(controllerContext, bindingContext);
        }

        /// <summary>
        /// After the model is updated, there may be a number of ModelState errors added by ASP.NET MVC for 
        /// and data casting problems that it runs into while binding the object.  This gets rid of those
        /// casting errors and uses the registered IValidator to populate the ModelState with any validation
        /// errors.
        /// </summary>
        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            foreach (string key in bindingContext.ModelState.Keys)
            {
                for (int i = 0; i < bindingContext.ModelState[key].Errors.Count; i++)
                {
                    ModelError modelError = bindingContext.ModelState[key].Errors[i];

                    // Get rid of all the MVC errors except those associated with parsing info; e.g., parsing DateTime fields
                    if (IsModelErrorAddedByMvc(modelError) && !IsMvcModelBinderFormatException(modelError))
                    {
                        bindingContext.ModelState[key].Errors.RemoveAt(i);
                        // Decrement the counter since we've shortened the list
                        i--;
                    }
                }
            }

            // Transfer any errors exposed by IValidator to the ModelState
            if (bindingContext.Model is IValidatable)
            {
                MvcValidationAdapter.TransferValidationMessagesTo(
                    bindingContext.ModelName, bindingContext.ModelState,
                    ((IValidatable)bindingContext.Model).ValidationResults());
            }
        }

        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            Type propertyType = propertyDescriptor.PropertyType;

            if ( IsEntityType(propertyType) )
            {
                ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                EntityValueProviderResult entityValueProviderResult = new EntityValueProviderResult(valueProviderResult, propertyType);

                return entityValueProviderResult.ConvertTo(propertyType);
            }
            else if ( IsSimpleGenericBindableEntityCollection(propertyType) )
            {
                ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                EntityCollectionValueProviderResult entityCollectionValueProviderResult = new EntityCollectionValueProviderResult(valueProviderResult, propertyType);

                return entityCollectionValueProviderResult.ConvertTo(propertyType);
            }

            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }



        /// <summary>
        /// The base implementation of this uses IDataErrorInfo to check for validation errors and 
        /// adds them to the ModelState. This override prevents that from occurring by doing nothing at all.
        /// </summary>
        protected override void OnPropertyValidated(ControllerContext controllerContext,
            ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
        }

        protected override void SetProperty(ControllerContext controllerContext,
            ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            SetIdProperty(bindingContext, propertyDescriptor, value);
            SetEntityCollectionProperty(bindingContext, propertyDescriptor, value);

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }

        /// <summary>
        /// The base implementatoin of this looks to see if a property value provided via a form is 
        /// bindable to the property and adds an error to the ModelState if it's not.  For example, if 
        /// a text box is left blank and the binding property is of type int, then the base implementation
        /// will add an error with the message "A value is required." to the ModelState.  We don't want 
        /// this to occur as we want these type of validation problems to be verified by our business rules.
        /// </summary>
        protected override bool OnPropertyValidating(ControllerContext controllerContext,
            ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {

            return true;
        }

        private static bool IsModelErrorAddedByMvc(ModelError modelError)
        {
            return modelError.Exception != null &&
                modelError.Exception.GetType().Equals(typeof(InvalidOperationException));
        }

        private static bool IsMvcModelBinderFormatException(ModelError modelError)
        {
            return modelError.Exception != null &&
                modelError.Exception.InnerException != null &&
                modelError.Exception.InnerException.GetType().Equals(typeof(FormatException));
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
                 propertyType.GetGenericTypeDefinition() == typeof(ISet<>));

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

            if (propertyDescriptor.Name == ID_PROPERTY_NAME)
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
                if (idProperty == null)
                {
                    idProperty = bindingContext.ModelType
                        .GetProperty(propertyDescriptor.Name,
                            BindingFlags.Public | BindingFlags.Instance);
                }

                // Set the value of the protected Id property
                idProperty.SetValue(bindingContext.Model, typedId, null);
            }
        }

        /// <summary>
        /// If the property being bound is a simple, generic collection of entiy objects, then use 
        /// reflection to get past the protected visibility of the collection property, if necessary.
        /// </summary>
        private static void SetEntityCollectionProperty(ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor, object value)
        {

            if (value as IEnumerable != null &&
                IsSimpleGenericBindableEntityCollection(propertyDescriptor.PropertyType))
            {

                object entityCollection = propertyDescriptor.GetValue(bindingContext.Model);
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
        #endregion

        private const string ID_PROPERTY_NAME = "Id";
    }
}
