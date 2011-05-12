namespace Tests.SharpArch.Domain.Commands
{
    using NUnit.Framework;

  [TestFixture]
    public class CommandTests
    {
        [Test]
        public void IsValidReturnsTrueForValidCommand()
        {
            var command = new ValidCommand();
            Assert.IsTrue(command.IsValid());
        }

        [Test]
        public void IsValidReturnsFalseForInvalidCommand()
        {
            var command = new InvalidCommand();
            Assert.IsFalse(command.IsValid());
        }

        [Test]
        public void CanGetValidationResultsForInvalidCommand()
        {
            var command = new InvalidCommand();
            var results = command.ValidationResults();

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);

            var enumerator = results.GetEnumerator();
            enumerator.MoveNext();
            Assert.AreEqual("The Invalid field is required.", enumerator.Current.ErrorMessage);
        }
    }
}