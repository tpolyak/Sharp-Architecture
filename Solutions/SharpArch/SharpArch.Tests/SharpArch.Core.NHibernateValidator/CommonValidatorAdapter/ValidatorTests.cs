namespace Tests.SharpArch.Core.NHibernateValidator.CommonValidatorAdapter
{
    using System.Linq;

    using NHibernate.Validator.Constraints;

    using NUnit.Framework;

    using global::SharpArch.Core.NHibernateValidator.CommonValidatorAdapter;

    [TestFixture]
    public class ValidatorTests
    {
        [Test]
        public void CanRetriveValiationResults()
        {
            var validator = new Validator();

            var invalidObject = new SomeObject();
            var results = validator.ValidationResultsFor(invalidObject);

            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results.First().PropertyName, Is.EqualTo("Name"));
            Assert.That(results.First().ClassContext, Is.EqualTo(typeof(SomeObject)));
            Assert.That(results.First().Message, Is.EqualTo("Dude...the name please!!"));
        }

        [Test]
        public void CanValidateObject()
        {
            var validator = new Validator();

            var invalidObject = new SomeObject();
            Assert.That(validator.IsValid(invalidObject), Is.False);

            var validObject = new SomeObject { Name = string.Empty };
            Assert.That(validator.IsValid(validObject), Is.True);
        }

        private class SomeObject
        {
            [NotNull(Message = "Dude...the name please!!")]
            public string Name { get; set; }
        }
    }
}