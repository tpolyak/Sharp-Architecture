namespace Tests.SharpArch.Domain.Commands
{
    using System.ComponentModel.DataAnnotations;

    using global::SharpArch.Domain.Commands;

    public class InvalidCommand : CommandBase
    {
        [Required(ErrorMessage = "The Invalid field is required.")]
        public bool? Invalid { get; set; }

        [Range(100, 199, ErrorMessage = "The field InvalidInt must be between 100 and 199.")]
        public int InvalidInt { get; set; }
    }
}