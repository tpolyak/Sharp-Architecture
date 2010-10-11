namespace Tests.SharpArch.Core
{
    using System;

    using Castle.Windsor;

    using CommonServiceLocator.WindsorAdapter;

    using Microsoft.Practices.ServiceLocation;

    using NUnit.Framework;

    using global::SharpArch.Core;
    using global::SharpArch.Core.CommonValidator;
    using global::SharpArch.Core.NHibernateValidator.CommonValidatorAdapter;

    [TestFixture]
    public class SafeServiceLocatorTests
    {
        [Test]
        public void CanReturnServiceIfInitializedAndRegistered()
        {
            IWindsorContainer container = new WindsorContainer();
            container.AddComponent("validator", typeof(IValidator), typeof(Validator));
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

            var validatorService = SafeServiceLocator<IValidator>.GetService();

            Assert.That(validatorService, Is.Not.Null);
        }

        [SetUp]
        public void Setup()
        {
            ServiceLocator.SetLocatorProvider(null);
        }

        [Test]
        public void WillBeInformedIfServiceLocatorNotInitialized()
        {
            var exceptionThrown = false;

            try
            {
                SafeServiceLocator<IValidator>.GetService();
            }
            catch (NullReferenceException e)
            {
                exceptionThrown = true;
                Assert.That(e.Message.Contains("ServiceLocator has not been initialized"));
            }

            Assert.That(exceptionThrown);
        }

        [Test]
        public void WillBeInformedIfServiceNotRegistered()
        {
            var exceptionThrown = false;

            IWindsorContainer container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

            try
            {
                SafeServiceLocator<IValidator>.GetService();
            }
            catch (ActivationException e)
            {
                exceptionThrown = true;
                Assert.That(e.Message.Contains("IValidator could not be located"));
            }

            Assert.That(exceptionThrown);
        }
    }
}