namespace SharpArch.Domain
{
    using System;

    // End Check
    #region Exceptions

    /// <summary>
    ///     Exception raised when an assertion fails.
    /// </summary>
    public class AssertionException : DesignByContractException
    {
        /// <summary>
        ///     Assertion Exception.
        /// </summary>
        public AssertionException()
        {
        }

        /// <summary>
        ///     Assertion Exception.
        /// </summary>
        public AssertionException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Assertion Exception.
        /// </summary>
        public AssertionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    #endregion // Exception classes
}