using System.Web.Mvc;
using System.ComponentModel;
using SharpArch.Core.CommonValidator;
using SharpArch.Web.CommonValidator;
using System;
using SharpArch.Core.DomainModel;
using System.Linq;
using SharpArch.Core;

namespace SharpArch.Web.ModelBinder
{
    public class SharpModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// After the model is updated, there may be a number of ModelState errors added by ASP.NET MVC for 
        /// and data casting problems that it runs into while binding the object.  This gets rid of those
        /// casting errors and using the registered IValidator to populate the ModelState with any validation
        /// errors.
        /// </summary>
        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            foreach (string key in bindingContext.ModelState.Keys) {
                for (int i = 0; i < bindingContext.ModelState[key].Errors.Count; i++) {
                    ModelError modelError = bindingContext.ModelState[key].Errors[i];

                    // Get rid of all the MVC errors except those associated with parsing info; e.g., parsing DateTime fields
                    if (IsModelErrorAddedByMvc(modelError) && !IsMvcModelBinderFormatException(modelError)) {
                        bindingContext.ModelState[key].Errors.RemoveAt(i);
                        // Decrement the counter since we've shortened the list
                        i--;
                    }
                }
            }

            // Transfer any errors exposed by IValidator to the ModelState
            if (bindingContext.Model is IValidatable) {
                MvcValidationAdapter.TransferValidationMessagesTo(bindingContext.ModelState,
                    ((IValidatable)bindingContext.Model).ValidationResults());
            }
        }

        private bool IsModelErrorAddedByMvc(ModelError modelError) {
            return modelError.Exception != null &&
                modelError.Exception.GetType().Equals(typeof(InvalidOperationException));
        }

        private bool IsMvcModelBinderFormatException(ModelError modelError) {
            return modelError.Exception != null &&
                modelError.Exception.InnerException != null &&
                modelError.Exception.InnerException.GetType().Equals(typeof(FormatException));
        }

        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor) {
            Type propertyType = propertyDescriptor.PropertyType;

            if (propertyType.GetInterfaces()
                .Any(type => type.IsGenericType
                    && type.GetGenericTypeDefinition() == typeof(IEntityWithTypedId<>))) {

                ReplaceValueProviderWithEntityValueProvider(bindingContext, propertyDescriptor, propertyType);
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }

        private void ReplaceValueProviderWithEntityValueProvider(ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, Type propertyType) {
            string valueProviderKey =
                CreateSubPropertyName(bindingContext.ModelName, propertyDescriptor.Name);

            ValueProviderResult defaultResult;
            bool couldGetDefaultResult =
                bindingContext.ValueProvider.TryGetValue(valueProviderKey, out defaultResult);

            if (couldGetDefaultResult) {
                bindingContext.ValueProvider.Remove(valueProviderKey);
                bindingContext.ValueProvider.Add(valueProviderKey,
                    new EntityValueProviderResult(defaultResult, propertyType));
            }
        }

        /// <summary>
        /// The base implementation of this uses IDataErrorInfo to check for validation errors and 
        /// adds them to the ModelState. This override prevents that from occurring by doing nothing at all.
        /// </summary>
        protected override void OnPropertyValidated(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value) {
        }

        #region Overridable (but not yet overridden) Methods

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            return base.BindModel(controllerContext, bindingContext);
        }

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType) {
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }

        protected override PropertyDescriptorCollection GetModelProperties(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            return base.GetModelProperties(controllerContext, bindingContext);
        }

        protected override bool OnModelUpdating(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            return base.OnModelUpdating(controllerContext, bindingContext);
        }

        protected override bool OnPropertyValidating(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value) {
            return base.OnPropertyValidating(controllerContext, bindingContext, propertyDescriptor, value);
        }

        #endregion
    }
}
