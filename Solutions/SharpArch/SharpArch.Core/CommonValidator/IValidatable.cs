using System.Collections.Generic;

namespace SharpArch.Core.CommonValidator
{
    public interface IValidatable
    {
        bool IsValid();
        ICollection<IValidationResult> ValidationResults();
    }
}
