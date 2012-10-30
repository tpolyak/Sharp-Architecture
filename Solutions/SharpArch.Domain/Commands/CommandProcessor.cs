using System;
using System.Collections.Generic;

namespace SharpArch.Domain.Commands
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    ///     Processes commands by handling them by means one or more suitable command handlers.
    /// </summary>
    public class CommandProcessor : ICommandProcessor
    {
        /// <summary>
        ///     Processes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <param name="command">The command.</param>
        public void Process<TCommand>(TCommand command) where TCommand : ICommand
        {
            Validator.ValidateObject(command, new ValidationContext(command, null, null), true);

            var handlers = ServiceLocator.Current.GetAllInstances<ICommandHandler<TCommand>>();
            if (handlers == null || !handlers.Any())
            {
                throw new CommandHandlerNotFoundException(typeof(TCommand));
            }

            foreach (var handler in handlers)
            {
                handler.Handle(command);
            }
        }

        /// <summary>
        ///     Processes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <typeparam name="TResult">The command result type.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>A collection of command result values.</returns>
        public IEnumerable<TResult> Process<TCommand, TResult>(TCommand command) where TCommand : ICommand
        {
            Validator.ValidateObject(command, new ValidationContext(command, null, null), true);

            var handlers = ServiceLocator.Current.GetAllInstances<ICommandHandler<TCommand, TResult>>();
            if (handlers == null || !handlers.Any())
            {
                throw new CommandHandlerNotFoundException(typeof(TCommand), typeof(TResult));
            }

            foreach (var handler in handlers)
            {
                yield return handler.Handle(command);
            }
        }

        /// <summary>
        ///     Processes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <typeparam name="TResult">The command result type.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="resultHandler">The result handler which is called for each command handler that has handled the command.</param>
        public void Process<TCommand, TResult>(TCommand command, Action<TResult> resultHandler) where TCommand : ICommand
        {
            foreach (var result in Process<TCommand, TResult>(command))
            {
                resultHandler(result);
            }
        }
    }
}
