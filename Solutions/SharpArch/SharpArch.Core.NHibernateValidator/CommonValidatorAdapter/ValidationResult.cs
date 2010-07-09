namespace SharpArch.Core.NHibernateValidator.CommonValidatorAdapter
{
    using System;

    using NHibernate.Validator.Engine;

    using SharpArch.Core.CommonValidator;

    public class ValidationResult : IValidationResult
    {
        public ValidationResult(InvalidValue invalidValue)
        {
            Check.Require(invalidValue != null, "invalidValue may not be null");

            this.ClassContext = invalidValue.EntityType;
            this.PropertyName = invalidValue.PropertyName;
            this.Message = invalidValue.Message;
            this.InvalidValue = invalidValue;
            this.AttemptedValue = invalidValue.Value;
        }

        public virtual object AttemptedValue { get; protected set; }

        public virtual Type ClassContext { get; protected set; }

        /// <summary>
        ///     This is not defined by IValidationResult but is useful for applications which are 
        ///     strictly using NHibernate Validator and need additional information about the 
        ///     validation problem.
        /// </summary>
        public virtual InvalidValue InvalidValue { get; protected set; }

        public virtual string Message { get; protected set; }

        public virtual string PropertyName { get; protected set; }
    }
}