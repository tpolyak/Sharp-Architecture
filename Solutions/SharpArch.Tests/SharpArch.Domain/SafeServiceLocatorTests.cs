namespace Tests.SharpArch.Domain
{
    using System;
    using System.Collections.Generic;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using CommonServiceLocator.WindsorAdapter;

    using Microsoft.Practices.ServiceLocation;

    using NUnit.Framework;

    using global::SharpArch.Domain;
    using global::SharpArch.Domain.CommonValidator;

    [TestFixture]
    public class SafeServiceLocatorTests
    {
        #region Public Methods

        [Test]
        public void CanReturnServiceIfInitializedAndRegistered()
        {
            IWindsorContainer container = new WindsorContainer();
            container.Register(Component.For(typeof(IValidator)).ImplementedBy(typeof(Validator)).Named("validator"));
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

            IValidator validatorService = SafeServiceLocator<IValidator>.GetService();

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
            bool exceptionThrown = false;

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
            bool exceptionThrown = false;

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

        #endregion

        public class Validator : IValidator
        {
            public bool IsValid(object value)
            {
                throw new NotImplementedException();
            }

            public ICollection<IValidationResult> ValidationResultsFor(object value)
            {
                throw new NotImplementedException();
            }
        }
    }
}