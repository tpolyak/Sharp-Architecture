using System.Collections.Generic;
using System.Web.Mvc;
using NHibernate.Validator.Engine;

namespace SharpArch.Core.NHibernateValidator.ValidatorProvider
{
    /// <summary>
    /// Server side model validator for NHVal
    /// </summary>
    public class NHibernateValidatorModelValidator : ModelValidator
    {
        private readonly IClassValidator _validator;

        public NHibernateValidatorModelValidator(ModelMetadata metadata, ControllerContext controllerContext, IClassValidator validator)
            : base(metadata, controllerContext)
        {
            _validator = validator;
        }

        /// <summary>
        /// Validate the model associated with this validator
        /// </summary>
        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var validationResults = _validator.GetInvalidValues(Metadata.Model);

            foreach (var validationResult in validationResults)
            {
                yield return
                    new ModelValidationResult { MemberName = validationResult.PropertyName, Message = validationResult.Message };
            }
        }
    }
}