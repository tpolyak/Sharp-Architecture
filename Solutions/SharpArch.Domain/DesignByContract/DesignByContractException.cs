namespace SharpArch.Domain
{
    using System;

    /// <summary>
    ///     Exception raised when a contract is broken.
    ///     Catch this exception type if you wish to differentiate between 
    ///     any DesignByContract exception and other runtime exceptions.
    /// </summary>
    public class DesignByContractException : ApplicationException
    {
        protected DesignByContractException()
        {
        }

        protected DesignByContractException(string message)
            : base(message)
        {
        }

        protected DesignByContractException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}