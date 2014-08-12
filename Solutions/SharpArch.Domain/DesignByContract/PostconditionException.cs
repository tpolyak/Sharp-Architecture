namespace SharpArch.Domain
{
    using System;

    /// <summary>
    ///     Exception raised when a postcondition fails.
    /// </summary>
    public class PostconditionException : DesignByContractException
    {
        /// <summary>
        ///     Postcondition Exception.
        /// </summary>
        public PostconditionException()
        {
        }

        /// <summary>
        ///     Postcondition Exception.
        /// </summary>
        public PostconditionException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Postcondition Exception.
        /// </summary>
        public PostconditionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}