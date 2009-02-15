using System.Web.Mvc;
using SharpArch.Core;
using System;
using System.Collections.Generic;
using SharpArch.Core.CommonValidator;

namespace SharpArch.Web.CommonValidator
{
    public class MvcValidationAdapter
    {
        /// <summary>
        /// This acts as a more "manual" alternative to moving validation errors to the 
        /// <see cref="ModelStateDictionary" /> if you care to bypass the use of 
        /// <see cref="ValidatableModelBinder" />.  This typically wouldn't be used in conjunction
        /// with <see cref="ValidatableModelBinder" /> but as an alternative to it.
        /// </summary>
        public static ModelStateDictionary TransferValidationMessagesTo(
            ModelStateDictionary modelStateDictionary, IEnumerable<IValidationResult> validationResults) {

            Check.Require(modelStateDictionary != null, "modelStateDictionary may not be null");
            Check.Require(validationResults != null, "invalidValues may not be null");

            foreach (IValidationResult validationResult in validationResults) {
                Check.Require(validationResult.ClassContext != null,
                    "validationResult.ClassContext may not be null");

                modelStateDictionary.AddModelError(validationResult.ClassContext.Name +
                    (!string.IsNullOrEmpty(validationResult.PropertyName)
                        ? "." + validationResult.PropertyName
                        : ""),
                    validationResult.Message);
            }

            return modelStateDictionary;
        }
    }
}
