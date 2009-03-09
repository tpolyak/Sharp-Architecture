using NUnit.Framework;
using System;
using SharpArch.Core.CommonValidator;
using SharpArch.Core;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using CommonServiceLocator.WindsorAdapter;
using SharpArch.Core.NHibernateValidator.CommonValidatorAdapter;
using NUnit.Framework.SyntaxHelpers;

namespace Tests.SharpArch.Core
{
    [TestFixture]
    public class SafeServiceLocatorTests
    {
        [Test]
        public void WillBeInformedIfServiceLocatorNotInitialized() {
            bool exceptionThrown = false;

            try {
                SafeServiceLocator<IValidator>.GetService();
            }
            catch (NullReferenceException e) {
                exceptionThrown = true;
                Assert.That(e.Message.Contains("ServiceLocator has not been initialized"));
            }

            Assert.That(exceptionThrown);
        }

        [Test]
        public void WillBeInformedIfServiceNotRegistered() {
            bool exceptionThrown = false;

            IWindsorContainer container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

            try {
                SafeServiceLocator<IValidator>.GetService();
            }
            catch (ActivationException e) {
                exceptionThrown = true;
                Assert.That(e.Message.Contains("IValidator could not be located"));
            }

            Assert.That(exceptionThrown);
        }

        [Test]
        public void CanReturnServiceIfInitializedAndRegistered() {
            IWindsorContainer container = new WindsorContainer();
            container.AddComponent("validator", typeof(IValidator), typeof(Validator));
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

            IValidator validatorService = SafeServiceLocator<IValidator>.GetService();

            Assert.That(validatorService, Is.Not.Null);
        }

        [TearDown]
        public void TearDown() {
            ServiceLocator.SetLocatorProvider(null);
        }
    }
}
