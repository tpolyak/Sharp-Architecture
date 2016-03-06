// ReSharper disable CheckNamespace
namespace SharpArch.Domain
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

    /// <summary>
    ///    Design by Contract checks developed by http://www.codeproject.com/KB/cs/designbycontract.aspx.
    /// 
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Each method generates an exception or a trace assertion statement if the contract is
    ///         broken.
    ///     </para>
    ///     <para>
    ///         This example shows how to call the Require method. Assume DBC_CHECK_PRECONDITION is
    ///         defined.
    ///         <code>
    ///             public void Test(int x)
    ///             {
    ///                 try
    ///                 {
    ///                     Check.Require(x > 1, "x must be > 1");
    ///                 }
    ///                 catch (System.Exception ex)
    ///                 {
    ///                     Console.WriteLine(ex.ToString());
    ///                 }
    ///             }
    ///         </code>
    ///     </para>
    ///     <para>
    ///         If you wish to use trace assertion statements, intended for Debug scenarios,
    ///         rather than exception handling then set 
    ///         <code>Check.UseAssertions = true</code>
    ///     </para>
    ///     <para>
    ///         You can specify this in your application entry point and maybe make it
    ///         dependent on conditional compilation flags or configuration file settings, e.g.,
    ///         <code>
    ///             #if DBC_USE_ASSERTIONS
    ///             Check.UseAssertions = true;
    ///             #endif
    ///         </code>
    ///     </para>
    ///     <para>
    ///         You can direct output to a Trace listener. For example, you could insert
    ///         <code>
    ///             Trace.Listeners.Clear();
    ///             Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
    ///         </code>
    ///         or direct output to a file or the Event Log.
    ///         Note: For ASP.NET clients use the Listeners collection of the Debug, not the Trace,
    ///         object and, for a Release build, only exception-handling is possible.
    ///     </para>
    /// </remarks>
    [Obsolete("Will be removed in the next version. Consider using ReSharper or CodeContracts.")]
    [ExcludeFromCodeCoverage]
    public static class Check
    {
        private static bool useAssertions;

        /// <summary>
        ///     Gets or sets a value indicating whether to use Trace Assert statements instead of
        ///     exception handling. 
        /// </summary>
        /// <remarks>
        ///     The <see cref="Check"/> class uses exception handling by default.
        /// </remarks>
        public static bool UseAssertions
        {
            get
            {
                return useAssertions;
            }

            set
            {
                useAssertions = value;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether exception handling is being used.
        /// </summary>
        private static bool UseExceptions
        {
            get
            {
                return !useAssertions;
            }
        }

        /// <summary>
        ///     Checks whether the specified precondition assertion expression result is
        ///     <c>true</c> and throws an exception otherwise, using a specified message.
        /// </summary>
        /// <param name="assertion">The assertion expression result.</param>
        /// <param name="message">The message to use when throwing an exception in case the assertion fails.</param>
        public static void Assert(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion)
                {
                    throw new AssertionException(message);
                }
            }
            else
            {
                Trace.Assert(assertion, "Assertion: " + message);
            }
        }

        /// <summary>
        ///     Checks whether the specified precondition assertion expression result is
        ///     <c>true</c> and throws an exception otherwise, using a specified message.
        /// </summary>
        /// <param name="assertion">The assertion expression result.</param>
        /// <param name="message">The message to use when throwing an exception in case the assertion fails.</param>
        /// <param name="inner">The inner exception to pass in the exception that is thrown in case the assertion fails.</param>
        public static void Assert(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion)
                {
                    throw new AssertionException(message, inner);
                }
            }
            else
            {
                Trace.Assert(assertion, "Assertion: " + message);
            }
        }

        /// <summary>
        ///     Checks whether the specified precondition assertion expression result is
        ///     <c>true</c> and throws an exception otherwise.
        /// </summary>
        /// <param name="assertion">The assertion expression result.</param>
        public static void Assert(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion)
                {
                    throw new AssertionException("Assertion failed.");
                }
            }
            else
            {
                Trace.Assert(assertion, "Assertion failed.");
            }
        }

        /// <summary>
        ///     Checks whether the specified postcondition assertion expression result is
        ///     <c>true</c> and throws an exception otherwise, using a specified message.
        /// </summary>
        /// <param name="assertion">The assertion expression result.</param>
        /// <param name="message">The message to use when throwing an exception in case the assertion fails.</param>
        /// <exception cref="PostconditionException">If assertion is false.</exception>
        [ContractAnnotation("assertion:false => stop")]
        public static void Ensure(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion)
                {
                    throw new PostconditionException(message);
                }
            }
            else
            {
                Trace.Assert(assertion, "Postcondition: " + message);
            }
        }

        /// <summary>
        ///     Checks whether the specified postcondition assertion expression result is
        ///     <c>true</c> and throws an exception otherwise, using a specified message.
        /// </summary>
        /// <param name="assertion">The assertion expression result.</param>
        /// <param name="message">The message to use when throwing an exception in case the assertion fails.</param>
        /// <param name="inner">The inner exception to pass in the exception that is thrown in case the assertion fails.</param>
        public static void Ensure(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion)
                {
                    throw new PostconditionException(message, inner);
                }
            }
            else
            {
                Trace.Assert(assertion, "Postcondition: " + message);
            }
        }

        /// <summary>
        ///     Checks whether the specified postcondition assertion expression result is
        ///     <c>true</c> and throws an exception otherwise.
        /// </summary>
        /// <param name="assertion">The assertion expression result.</param>
        public static void Ensure(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion)
                {
                    throw new PostconditionException("Postcondition failed.");
                }
            }
            else
            {
                Trace.Assert(assertion, "Postcondition failed.");
            }
        }

        /// <summary>
        ///     Checks whether the specified invariant assertion expression result is
        ///     <c>true</c> and throws an exception otherwise, using a specified message.
        /// </summary>
        /// <param name="assertion">The assertion expression result.</param>
        /// <param name="message">The message to use when throwing an exception in case the assertion fails.</param>
        public static void Invariant(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion)
                {
                    throw new InvariantException(message);
                }
            }
            else
            {
                Trace.Assert(assertion, "Invariant: " + message);
            }
        }

        /// <summary>
        ///     Checks whether the specified invariant assertion expression result is
        ///     <c>true</c> and throws an exception otherwise, using a specified message.
        /// </summary>
        /// <param name="assertion">The assertion expression result.</param>
        /// <param name="message">The message to use when throwing an exception in case the assertion fails.</param>
        /// <param name="inner">The inner exception to pass in the exception that is thrown in case the assertion fails.</param>
        public static void Invariant(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion)
                {
                    throw new InvariantException(message, inner);
                }
            }
            else
            {
                Trace.Assert(assertion, "Invariant: " + message);
            }
        }

        /// <summary>
        ///     Checks whether the specified invariant assertion expression result is
        ///     <c>true</c> and throws an exception otherwise.
        /// </summary>
        /// <param name="assertion">The assertion expression result.</param>
        public static void Invariant(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion)
                {
                    throw new InvariantException("Invariant failed.");
                }
            }
            else
            {
                Trace.Assert(assertion, "Invariant failed.");
            }
        }

        /// <summary>
        ///     Checks whether the specified precondition assertion expression result is
        ///     <c>true</c> and throws an exception otherwise, using a specified message.
        /// </summary>
        /// <param name="assertion">The assertion expression result.</param>
        /// <param name="message">The message to use when throwing an exception in case the assertion fails.</param>
        /// <exception cref="PreconditionException">Assertion is false.</exception>
        /// <remarks>
        ///     This should run regardless of preprocessor directives.
        /// </remarks>
        [ContractAnnotation("assertion:false => stop")]
        public static void Require(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion)
                {
                    throw new PreconditionException(message);
                }
            }
            else
            {
                Trace.Assert(assertion, "Precondition: " + message);
            }
        }

        /// <summary>
        ///     Checks whether the specified precondition assertion expression result is
        ///     <c>true</c> and throws an exception otherwise, using a specified message.
        /// </summary>
        /// <param name="assertion">The assertion expression result.</param>
        /// <param name="message">The message to use when throwing an exception in case the assertion fails.</param>
        /// <param name="inner">The inner exception to pass in the exception that is thrown in case the assertion fails.</param>
        /// <remarks>
        ///     This should run regardless of preprocessor directives.
        /// </remarks>
        public static void Require(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion)
                {
                    throw new PreconditionException(message, inner);
                }
            }
            else
            {
                Trace.Assert(assertion, "Precondition: " + message);
            }
        }

        /// <summary>
        /// Checks whether the specified precondition assertion expression result is
        /// <c>true</c> and throws an exception otherwise.
        /// </summary>
        /// <param name="assertion">The assertion expression result.</param>
        /// <exception cref="PreconditionException">Precondition failed.</exception>
        /// <remarks>
        /// This should run regardless of preprocessor directives.
        /// </remarks>
        [ContractAnnotation("false => stop")]
        public static void Require(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion)
                {
                    throw new PreconditionException("Precondition failed.");
                }
            }
            else
            {
                Trace.Assert(assertion, "Precondition failed.");
            }
        }
    }
}