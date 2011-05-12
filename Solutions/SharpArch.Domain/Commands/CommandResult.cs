namespace SharpArch.Domain.Commands
{
    public class CommandResult : ICommandResult
    {
        protected CommandResult(bool success)
        {
            this.Success = success;
        }

        public bool Success { get; protected set; }
    }
}