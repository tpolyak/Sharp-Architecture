namespace SharpArch.Domain
{
    using System;

    /// <summary>
    ///     Exception raised when a precondition fails.
    /// </summary>
    public class PreconditionException : DesignByContractException
    {
        /// <summary>
        ///     Precondition Exception.
        /// </summary>
        public PreconditionException()
        {
        }

        /// <summary>
        ///     Precondition Exception.
        /// </summary>
        public PreconditionException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Precondition Exception.
        /// </summary>
        public PreconditionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}