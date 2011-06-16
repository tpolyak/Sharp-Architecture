namespace SharpArch.Domain.Commands
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Microsoft.Practices.ServiceLocation;

    public class CommandProcessor : ICommandProcessor
    {
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
    }
}
