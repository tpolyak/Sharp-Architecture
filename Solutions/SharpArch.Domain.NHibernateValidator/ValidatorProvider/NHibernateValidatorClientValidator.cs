namespace SharpArch.Domain.NHibernateValidator.ValidatorProvider
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    ///     Simple validator class which overrides GetClientValidationRules to return a list of ModelClientValidationRules, which cause client side validation
    /// </summary>
    internal class NHibernateValidatorClientValidator : ModelValidator
    {
        public NHibernateValidatorClientValidator(
            ModelMetadata metadata, ControllerContext controllerContext, List<ModelClientValidationRule> rules)
            : base(metadata, controllerContext)
        {
            this.Rules = rules;
        }

        protected List<ModelClientValidationRule> Rules { get; set; }

        /// <summary>
        ///     Simply returns the supplied list of ModelClientValidationRule instances.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return this.Rules;
        }

        /// <summary>
        ///     Returns an empty enumeration since this is not a server-side validator
        /// </summary>
        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            return Enumerable.Empty<ModelValidationResult>();
        }
    }
}