namespace SharpArch.Domain.Commands
{
    public interface ICommandResults
    {
        bool Success { get; }

        ICommandResult[] Results { get; }
    }
}