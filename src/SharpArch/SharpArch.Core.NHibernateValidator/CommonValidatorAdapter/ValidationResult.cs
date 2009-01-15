using NHibernate.Validator.Engine;
using System;
using SharpArch.Core.CommonValidator;

namespace SharpArch.Core.NHibernateValidator.CommonValidatorAdapter
{
    public class ValidationResult : SharpArch.Core.CommonValidator.IValidationResult
    {
        public ValidationResult(InvalidValue invalidValue) {
            Check.Require(invalidValue != null, "invalidValue may not be null");

            ClassContext = invalidValue.BeanClass;
            PropertyName = invalidValue.PropertyName;
            Message = invalidValue.Message;
        }

        public virtual Type ClassContext { get; protected set; }
        public virtual string PropertyName { get; protected set; }
        public virtual string Message { get; protected set; }
    }
}
