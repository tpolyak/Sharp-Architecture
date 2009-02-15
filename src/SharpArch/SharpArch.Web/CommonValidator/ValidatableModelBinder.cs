using System.Web.Mvc;
using System.ComponentModel;
using SharpArch.Core.CommonValidator;

namespace SharpArch.Web.CommonValidator
{
    /// <summary>
    /// With ASP.NET MVC RC, Microsoft introduced IDataErrorInfo which allows one to enforce 
    /// validation with the raising of exceptions to communicate information about the invalid state.
    /// 
    /// This model binder class provides an alternative to raising exceptions by tracking validation 
    /// issues during the binding process itself.  More information about this approach is described
    /// by David Hayden at http://codebetter.com/blogs/david.hayden/archive/2009/02/03/an-aha-moment-on-mvc-validation-extensibility-in-defaultmodelbinder-bye-to-idataerrorinfo.aspx.
    /// </summary>
    public class ValidatableModelBinder : DefaultModelBinder
    {
        protected override void OnModelUpdated(ControllerContext controllerContext, 
            ModelBindingContext bindingContext) {

            IValidatable model = bindingContext.Model as IValidatable;

            if (model != null) {
                if (! model.IsValid()) {
                    foreach (IValidationResult validationResult in model.ValidationResults()) {
                        bindingContext.ModelState
                            .AddModelError(validationResult.ClassContext.Name +
                            (!string.IsNullOrEmpty(validationResult.PropertyName)
                                ? "." + validationResult.PropertyName
                                : ""),
                        validationResult.Message);
                    }
                }
            }
        }

        protected override void OnPropertyValidated(ControllerContext controllerContext, 
            ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value) {
            // Do nothing
        }
    }
}
