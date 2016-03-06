// ReSharper disable CheckNamespace
namespace SharpArch.Domain
{
    using System;

    /// <summary>
    ///     An exception that is raised when a contract is broken.
    /// </summary>
    /// <remarks>
    ///     Catch this exception type if you wish to differentiate between 
    ///     any design by contract exception and other runtime exceptions.
    /// </remarks>
    [Serializable]
    public class DesignByContractException : ApplicationException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DesignByContractException" /> class.
        /// </summary>
        protected DesignByContractException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DesignByContractException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        protected DesignByContractException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DesignByContractException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        protected DesignByContractException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}