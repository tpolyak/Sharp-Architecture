namespace SharpArch.Domain.CommonValidator
{
    using System.Collections.Generic;

    public interface IValidator
    {
        bool IsValid(object value);

        ICollection<IValidationResult> ValidationResultsFor(object value);
    }
}