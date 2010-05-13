using System.Collections.Generic;
using System.Web.Mvc;

namespace SharpArch.Core.NHibernateValidator.ValidatorProvider
{
    /// <summary>
    /// Server side validator provider for NHVal
    /// </summary>
    public class NHibernateValidatorProvider : ModelValidatorProvider
    {
        /// <summary>
        /// Returns model validators for each class that can be validated.
        /// When this method is called with a non-class modelType, nothing is added to the yield return
        /// (this prevents us from validating the same properties several times)
        /// </summary>
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            var validationEngine = ValidatorEngineFactory.ValidatorEngine;

            var classValidator = validationEngine.GetClassValidator(metadata.ModelType);

            if (classValidator != null)
                yield return new NHibernateValidatorModelValidator(metadata, context, classValidator);
        }
    }
}