namespace Tests.SharpArch.Domain
{
    using System;

    using NUnit.Framework;

    using global::SharpArch.Domain;

    [TestFixture]
    public class DesignByContractTests
    {
        [Test]
        public void CanEnforcePostcondition()
        {
            Assert.Throws<PostconditionException>(() => this.ConvertToPercentage(2m));
        }

        [Test]
        public void CanEnforcePrecondition()
        {
            Assert.Throws<PreconditionException>(() => this.ConvertToPercentage(-.2m));
        }

        [Test]
        public void CanGetPastPreconditionAndPostCondition()
        {
            Assert.AreEqual("20%", this.ConvertToPercentage(.2m));
        }

        private string ConvertToPercentage(decimal fractionalPercentage)
        {
            Check.Require(fractionalPercentage > 0, "fractionalPercentage must be > 0");

            var convertedValue = fractionalPercentage * 100;

            // Yes, I could have enforced this outcome in the precondition, but then you
            // wouldn't have seen the Check.Ensure in action, now would you?
            Check.Ensure(
                convertedValue >= 0 && convertedValue <= 100, 
                "convertedValue was expected to be within 0-100, but was " + convertedValue);

            return Math.Round(convertedValue) + "%";
        }
    }
}