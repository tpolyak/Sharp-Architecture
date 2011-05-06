namespace SharpArch.Domain.Commands
{
    public class CommandResultBase : ICommandResult
    {
        protected CommandResultBase(bool success)
        {
            this.Success = success;
        }

        public bool Success { get; protected set; }
    }
}