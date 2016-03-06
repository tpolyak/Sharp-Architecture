// ReSharper disable CheckNamespace
namespace SharpArch.Domain
{
    using System;

    // End Check
    #region Exceptions

    /// <summary>
    ///     An exception that is raised when an assertion check fails.
    /// </summary>
    [Serializable]
    public class AssertionException : DesignByContractException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AssertionException" /> class.
        /// </summary>
        public AssertionException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssertionException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AssertionException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssertionException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public AssertionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    #endregion // Exception classes
}