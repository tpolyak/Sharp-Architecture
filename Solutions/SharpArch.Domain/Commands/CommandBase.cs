namespace SharpArch.Domain.Commands
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Serves as the base class for commands.
    /// </summary>
    public class CommandBase : ICommand
    {
        /// <summary>
        ///     Determines whether the command is valid.
        /// </summary>
        /// <returns><c>true</c> if the command is valid; otherwise, <c>false</c>.</returns>
        public virtual bool IsValid()
        {
            return ValidationResults().Count == 0;
        }

        /// <summary>
        ///     Validates all properties of the command and returns the validation results if any of
        ///     them were deemed invalid.
        /// </summary>
        /// <returns>A collection of validation results.</returns>
        public virtual ICollection<ValidationResult> ValidationResults()
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this, null, null), validationResults, true);
            return validationResults;
        }
    }
}