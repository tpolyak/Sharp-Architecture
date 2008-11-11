using NUnit.Framework;
using NHibernate.Validator.Engine;
using SharpArch.Web.NHibernate.Validator;
using System.Web.Mvc;
using NUnit.Framework.SyntaxHelpers;
using SharpArch.Web.NHibernate;

namespace Tests.SharpArch.Web.NHibernate.Validator
{
    [TestFixture]
    public class MvcValidationAdapterTests
    {
        [Test]
        public void CanTransferValidationMessagesToModelState() {
            ModelStateDictionary modelStateDictionary = new ModelStateDictionary();
            InvalidValue[] invalidValues = new InvalidValue[3];
            invalidValues[0] = new InvalidValue("Message 1", typeof(TransactionAttribute), "Property1", null, null);
            invalidValues[1] = new InvalidValue("Message 2", typeof(MvcValidationAdapter), "Property2", null, null);
            invalidValues[2] = new InvalidValue("Message 3", GetType(), "Property3", null, null);

            MvcValidationAdapter.TransferValidationMessagesTo(modelStateDictionary, invalidValues);

            Assert.That(modelStateDictionary.Count, Is.EqualTo(3));
            Assert.That(modelStateDictionary["TransactionAttribute.Property1"].Errors[0].ErrorMessage, 
                Is.EqualTo("Message 1"));
            Assert.That(modelStateDictionary["MvcValidationAdapter.Property2"].Errors[0].ErrorMessage, 
                Is.EqualTo("Message 2"));
            Assert.That(modelStateDictionary["MvcValidationAdapterTests.Property3"].Errors[0].ErrorMessage, 
                Is.EqualTo("Message 3"));
        }
    }
}
