namespace Tests.SharpArch.Domain.Commands
{
    using global::SharpArch.Domain.Commands;

    public class TestCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        public delegate ICommandResult OnHandle(ICommandHandler<TCommand> handler, TCommand command);
        public event OnHandle OnHandleEvent;

        public TestCommandHandler(OnHandle onHandle)
        {
            OnHandleEvent += onHandle;
        }

        public ICommandResult Handle(TCommand command)
        {
            var onHandleEvent = OnHandleEvent;
            return onHandleEvent != null ? onHandleEvent(this, command) : null;
        }
    }
}