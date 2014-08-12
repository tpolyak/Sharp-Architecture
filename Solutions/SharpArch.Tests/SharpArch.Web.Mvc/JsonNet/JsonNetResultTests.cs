namespace Tests.SharpArch.Web.Mvc.JsonNet
{
    using System.Text;

    using NUnit.Framework;

    using global::SharpArch.Web.Mvc.JsonNet;

    [TestFixture]
    public class JsonNetResultTests
    {
        [Test]
        public void CanCreateJsonNetResultUsingOverloadedConstructors()
        {
            JsonNetResult result;

            result = new JsonNetResult();
            Assert.That(result.SerializerSettings, Is.Not.Null);

            // This is the most typical scenario
            result = new JsonNetResult("data to be serialized");
            Assert.That(result.SerializerSettings, Is.Not.Null);
            Assert.That(result.Data, Is.Not.Null);

            // Not used very often, if ever
            result = new JsonNetResult("hi", "whatever");
            Assert.That(result.SerializerSettings, Is.Not.Null);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.ContentType, Is.Not.Null);

            // Not used very often, if ever
            result = new JsonNetResult("hi", "whatever", Encoding.UTF8);
            Assert.That(result.SerializerSettings, Is.Not.Null);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.ContentType, Is.Not.Null);
            Assert.That(result.ContentEncoding, Is.EqualTo(Encoding.UTF8));
        }
    }
}