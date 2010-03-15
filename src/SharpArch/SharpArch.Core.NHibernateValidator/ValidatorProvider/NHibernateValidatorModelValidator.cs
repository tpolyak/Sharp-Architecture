using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Validator.Engine;

namespace SharpArch.Core.NHibernateValidator.ValidatorProvider
{
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

            return validationResults.Select(validationResult => new ModelValidationResult
                                                                    {
                                                                        MemberName = validationResult.PropertyName,
                                                                        Message = validationResult.Message
                                                                    });
        }
    }
}
