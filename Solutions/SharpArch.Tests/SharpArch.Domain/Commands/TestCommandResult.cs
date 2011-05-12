namespace Tests.SharpArch.Domain.Commands
{
    using global::SharpArch.Domain.Commands;

    public class TestCommandResult : CommandResult
    {
        public TestCommandResult(bool success) : base(success) {}
    }
}