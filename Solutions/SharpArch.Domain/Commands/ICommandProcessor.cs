using System;
using System.Collections.Generic;

namespace SharpArch.Domain.Commands
{
    /// <summary>
    ///     Defines the public members of a command processor, which processes commands by handling
    ///     them by means one or more suitable command handlers.
    /// </summary>
    public interface ICommandProcessor
    {
        /// <summary>
        ///     Processes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <param name="command">The command.</param>
        void Process<TCommand>(TCommand command) where TCommand : ICommand;

        /// <summary>
        ///     Processes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <typeparam name="TResult">The command result type.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>A collection of command result values.</returns>
        IEnumerable<TResult> Process<TCommand, TResult>(TCommand command) where TCommand : ICommand;

        /// <summary>
        ///     Processes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <typeparam name="TResult">The command result type.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="resultHandler">The result handler which is called for each command handler that has handled the command.</param>
        void Process<TCommand, TResult>(TCommand command, Action<TResult> resultHandler) where TCommand : ICommand;
    }
}