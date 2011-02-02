namespace SharpArch.Domain.NHibernateValidator.CommonValidatorAdapter
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using NHibernate.Validator.Engine;

    using SharpArch.Domain.CommonValidator;
    using SharpArch.Infrastructure.NHibernate;

    using IValidator = SharpArch.Domain.CommonValidator.IValidator;

    /// <summary>
    ///     Provides an implementation of the <see cref = "CommonValidator.IValidator" /> interface 
    ///     which relies on NHibernate validator
    /// </summary>
    public class Validator : IValidator
    {
        private static readonly ValidatorEngine ValidatorEngine;

        static Validator()
        {
            ValidatorEngine = NHibernateSession.ValidatorEngine ?? new ValidatorEngine();
        }

        public bool IsValid(object value)
        {
            Check.Require(value != null, "value to IsValid may not be null");

            return ValidatorEngine.IsValid(value);
        }

        public ICollection<IValidationResult> ValidationResultsFor(object value)
        {
            Check.Require(value != null, "value to ValidationResultsFor may not be null");

            var invalidValues = ValidatorEngine.Validate(value);

            return ParseValidationResultsFrom(invalidValues);
        }

        private static ICollection<IValidationResult> ParseValidationResultsFrom(IEnumerable<InvalidValue> invalidValues)
        {
            ICollection<IValidationResult> validationResults = new Collection<IValidationResult>();

            foreach (var invalidValue in invalidValues)
            {
                validationResults.Add(new ValidationResult(invalidValue));
            }

            return validationResults;
        }
    }
}