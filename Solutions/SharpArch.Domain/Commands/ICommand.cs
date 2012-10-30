namespace SharpArch.Domain.Commands
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Defines the public members of a command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        ///     Determines whether the command is valid.
        /// </summary>
        /// <returns><c>true</c> if the command is valid; otherwise, <c>false</c>.</returns>
        bool IsValid();

        /// <summary>
        ///     Validates all properties of the command and returns the validation results if any of
        ///     them were deemed invalid.
        /// </summary>
        /// <returns>A collection of validation results.</returns>
        ICollection<ValidationResult> ValidationResults();
    }
}