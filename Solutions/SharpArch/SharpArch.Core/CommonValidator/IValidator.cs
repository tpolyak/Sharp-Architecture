using System.Collections.Generic;

namespace SharpArch.Core.CommonValidator
{
    public interface IValidator
    {
        bool IsValid(object value);
        ICollection<IValidationResult> ValidationResultsFor(object value);
    }
}
