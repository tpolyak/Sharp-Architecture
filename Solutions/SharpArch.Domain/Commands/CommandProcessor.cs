using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace SharpArch.Domain.Commands
{
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    ///     Processes commands by handling them by means of a suitable command handler.
    /// </summary>
    public class CommandProcessor : ICommandProcessor
    {
        /// <summary>
        ///     Processes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <param name="command">The command.</param>
        public virtual void Process<TCommand>(TCommand command) where TCommand : ICommand
        {
            Validator.ValidateObject(command, new ValidationContext(command, null, null), true);

            ICommandHandler<TCommand> handler = GetHandler<ICommandHandler<TCommand>, TCommand>(command);
            handler.Handle(command);
        }

        /// <summary>
        ///     Processes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <typeparam name="TResult">The command result type.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="resultHandler">The result handler which is called for the command handler that has handled the command.</param>
        /// <returns>The command result.</returns>
        public virtual TResult Process<TCommand, TResult>(TCommand command, Action<TResult> resultHandler = null) where TCommand : ICommand
        {
            Validator.ValidateObject(command, new ValidationContext(command, null, null), true);

            ICommandHandler<TCommand, TResult> handler = GetHandler<ICommandHandler<TCommand, TResult>, TCommand>(command);

            TResult result = handler.Handle(command);

            if (resultHandler != null)
            {
                resultHandler(result);
            }

            return result;
        }

        /// <summary>
        ///     Returns a suitable handler that can handle the specified command.
        /// </summary>
        /// <typeparam name="THandler">The handler type.</typeparam>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>A command handler.</returns>
        /// <exception cref="CommandHandlerNotFoundException">Thrown if no command handler could be found to handle the command.</exception>
        /// <exception cref="InvalidOperationException">Thrown if more than one suitable command handler was found.</exception>
        private static THandler GetHandler<THandler, TCommand>(TCommand command) where TCommand : ICommand
        {
            var handlers = ServiceLocator.Current.GetAllInstances<THandler>();

            if (handlers == null || !handlers.Any())
            {
                throw new CommandHandlerNotFoundException(typeof(TCommand));
            }
            else if (handlers.Count() > 1)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Multiple command handlers found for command {0}.", command.GetType()));
            }

            return handlers.First();
        }
    }
}
