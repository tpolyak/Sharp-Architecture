using System;

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
        void Process<TCommand>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        ///     Processes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <typeparam name="TResult">The command result type.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="resultHandler">The result handler which is called for the command handler that has handled the command.</param>
        /// <returns>The command result.</returns>
        TResult Process<TCommand, TResult>(TCommand command, Action<TResult> resultHandler = null)
            where TCommand : ICommand;
    }
}