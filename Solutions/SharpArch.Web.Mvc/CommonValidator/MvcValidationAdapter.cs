/*
namespace SharpArch.Web.Mvc.CommonValidator
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;

    using SharpArch.Domain;
    using SharpArch.Domain.CommonValidator;

    public class MvcValidationAdapter
    {
        public static ModelStateDictionary TransferValidationMessagesTo(
            ModelStateDictionary modelStateDictionary, IEnumerable<IValidationResult> validationResults)
        {
            return TransferValidationMessagesTo(null, modelStateDictionary, validationResults);
        }

        /// <summary>
        ///     This acts as a more "manual" alternative to moving validation errors to the 
        ///     <see cref = "ModelStateDictionary" /> if you care to bypass the use of 
        ///     <see cref = "ValidatableModelBinder" />.  This typically wouldn't be used in conjunction
        ///     with <see cref = "ValidatableModelBinder" /> but as an alternative to it.
        /// </summary>
        /// <param name = "keyBase">If supplied, will be used as the model state prefix instead of the class name</param>
        /// <param name="modelStateDictionary">The model state dictionary that will be populated with the validation results.</param>
        /// <param name="validationResults">The results to transfer.</param>
        public static ModelStateDictionary TransferValidationMessagesTo(
            string keyBase, 
            ModelStateDictionary modelStateDictionary, 
            IEnumerable<IValidationResult> validationResults)
        {
            Check.Require(modelStateDictionary != null, "modelStateDictionary may not be null");
            Check.Require(validationResults != null, "invalidValues may not be null");

            foreach (var validationResult in validationResults)
            {
                Check.Require(validationResult.ClassContext != null, "validationResult.ClassContext may not be null");

                var key = (keyBase ?? validationResult.ClassContext.Name) +
                          (!string.IsNullOrEmpty(validationResult.PropertyName)
                               ? "." + validationResult.PropertyName
                               : string.Empty);
                key = key.Trim('.');
                    
                // fixes issue where key ends up as .PropertyName instead of object.PropertyName or just PropertyName
                modelStateDictionary.AddModelError(key, validationResult.Message);
                
                var valueProviderResult = new ValueProviderResult(
                    validationResult.AttemptedValue ?? string.Empty, 
                    (validationResult.AttemptedValue ?? string.Empty).ToString(), 
                    CultureInfo.CurrentCulture);

                modelStateDictionary.SetModelValue(
                    key, 
                    valueProviderResult); // Fixes problem where invalid fields get blanked out.

                // modelStateDictionary.SetModelValue(key, new ValueProviderResult(null, null, null));
            }

            return modelStateDictionary;
        }
    }
}
*/