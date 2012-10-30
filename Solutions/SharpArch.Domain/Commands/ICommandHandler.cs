namespace SharpArch.Domain.Commands
{
    /// <summary>
    ///     Defines the public members of a command handler that does not return a result value.
    /// </summary>
    /// <typeparam name="TCommand">The type of the T command.</typeparam>
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        /// <summary>
        ///     Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        void Handle(TCommand command);
    }

    /// <summary>
    ///     Defines the public members of a command handler that returns a result value.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    /// <typeparam name="TResult">The command result type.</typeparam>
    public interface ICommandHandler<in TCommand, out TResult> where TCommand : ICommand
    {
        /// <summary>
        ///     Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The handler result.</returns>
        TResult Handle(TCommand command);
    }
}