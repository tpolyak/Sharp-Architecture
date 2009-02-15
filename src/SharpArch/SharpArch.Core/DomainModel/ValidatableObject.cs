using SharpArch.Core.CommonValidator;
using System;
using System.Collections.Generic;

namespace SharpArch.Core.DomainModel
{
    [Serializable]
    public abstract class ValidatableObject : BaseObject, IValidatable
    {
        public virtual bool IsValid() {
            return Validator.IsValid(this);
        }

        public virtual ICollection<IValidationResult> ValidationResults() {
            return Validator.ValidationResultsFor(this);
        }

        private IValidator Validator {
            get {
                if (validator == null) {
                    validator = SafeServiceLocator<IValidator>.GetService();
                }

                return validator;
            }
        }

        [ThreadStatic]
        private static IValidator validator;
    }
}
