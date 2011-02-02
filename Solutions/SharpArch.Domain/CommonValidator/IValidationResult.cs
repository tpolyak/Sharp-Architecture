namespace SharpArch.Domain.CommonValidator
{
    using System;
    using System.Reflection;

    public interface IValidationResult
    {
        /// <summary>
        ///     The value that was determined to be invalid
        /// </summary>
        object AttemptedValue { get; }

        /// <summary>
        ///     This is the class type that the validation result is applicable to.  For instance,
        ///     if the validation result concerns a duplicate record found for an employee, then 
        ///     this property would hold the typeof(Employee).  It should be expected that this 
        ///     property will never be null.
        /// </summary>
        Type ClassContext { get; }

        /// <summary>
        ///     Holds the message describing the validation result for the ClassContext and/or PropertyContext
        /// </summary>
        string Message { get; }

        /// <summary>
        ///     If the validation result is applicable to a specific property, then this 
        ///     <see cref = "PropertyInfo" /> would be set to a property name.
        /// </summary>
        string PropertyName { get; }
    }
}