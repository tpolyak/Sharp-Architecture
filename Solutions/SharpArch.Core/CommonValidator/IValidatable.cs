namespace SharpArch.Core.CommonValidator
{
    using System.Collections.Generic;

    public interface IValidatable
    {
        bool IsValid();

        ICollection<IValidationResult> ValidationResults();
    }
}