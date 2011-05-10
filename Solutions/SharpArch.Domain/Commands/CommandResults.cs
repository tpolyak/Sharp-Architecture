namespace SharpArch.Domain.Commands
{
    using System.Collections.Generic;
    using System.Linq;

    public class CommandResults : ICommandResults
    {
        private readonly List<ICommandResult> results = new List<ICommandResult>();

        public bool Success
        {
            get
            {
                return this.results.All(result => result.Success);
            }
        }

        public ICommandResult[] Results
        {
            get
            {
                return this.results.ToArray();
            }
        }

        public void AddResult(ICommandResult result)
        {
            this.results.Add(result);
        }
    }
}