namespace SharpArch.Domain.NHibernateValidator.ValidatorProvider
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using NHibernate.Validator.Engine;

    /// <summary>
    ///     Server side model validator for NHVal
    /// </summary>
    internal class NHibernateValidatorModelValidator : ModelValidator
    {
        private readonly IClassValidator validator;

        public NHibernateValidatorModelValidator(
            ModelMetadata metadata, ControllerContext controllerContext, IClassValidator validator)
            : base(metadata, controllerContext)
        {
            this.validator = validator;
        }

        /// <summary>
        ///     Validate the model associated with this validator
        /// </summary>
        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var validationResults = this.validator.GetInvalidValues(this.Metadata.Model);

            foreach (var validationResult in validationResults)
            {
                yield return
                    new ModelValidationResult
                        {
                           MemberName = validationResult.PropertyName, Message = validationResult.Message 
                        };
            }
        }
    }
}