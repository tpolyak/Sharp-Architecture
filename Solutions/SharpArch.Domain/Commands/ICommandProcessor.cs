namespace SharpArch.Domain.Commands
{
    public interface ICommandProcessor
    {
        ICommandResults Process<TCommand>(TCommand command) where TCommand : ICommand;
    }
}