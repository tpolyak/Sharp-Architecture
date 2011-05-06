namespace SharpArch.Domain.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        ICommandResult Handle(TCommand command);
    }
}