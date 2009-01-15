using System.Web.Mvc;
using SharpArch.Core;
using System;
using System.Collections.Generic;
using SharpArch.Core.CommonValidator;

namespace SharpArch.Web.CommonValidator
{
    public class MvcValidationAdapter
    {
        public static ModelStateDictionary TransferValidationMessagesTo(ModelStateDictionary modelStateDictionary,
            IEnumerable<IValidationResult> validationResults) {
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
