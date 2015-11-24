namespace Tests.SharpArch.Domain.DataAnnotationsValidator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using CommonServiceLocator.WindsorAdapter;

    using global::SharpArch.Domain;
    using global::SharpArch.Domain.DomainModel;
    using global::SharpArch.Domain.PersistenceSupport;
    using global::SharpArch.NHibernate.NHibernateValidator;

    using Microsoft.Practices.ServiceLocation;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class HasUniqueObjectSignatureValidatorTests
    {
        private Mock<IServiceProvider> serviceProviderMock;

        [Test]
        public void CanVerifyThatDuplicateExistsDuringValidationProcess()
        {
            var contractor = new Contractor { Name = "Codai" };
            var validationContext = ValidationContextFor(contractor);
            IEnumerable<ValidationResult> invalidValues = contractor.ValidationResults(validationContext);

            Assert.That(contractor.IsValid(validationContext), Is.False);

            foreach (var invalidValue in invalidValues)
            {
                Debug.WriteLine(invalidValue.ErrorMessage);
            }
        }

        [Test]
        public void CanVerifyThatDuplicateExistsOfEntityWithGuidIdDuringValidationProcess()
        {
            var objectWithGuidId = new ObjectWithGuidId { Name = "codai" };

            Assert.That(objectWithGuidId.IsValid(ValidationContextFor(objectWithGuidId)), Is.False);

            objectWithGuidId = new ObjectWithGuidId { Name = "whatever" };
            Assert.That(objectWithGuidId.IsValid(ValidationContextFor(objectWithGuidId)), Is.True);
        }

        [Test]
        public void CanVerifyThatDuplicateExistsOfEntityWithStringIdDuringValidationProcess()
        {
            var user = new User { SSN = "123-12-1234" };
            Assert.That(user.IsValid(ValidationContextFor(user)), Is.False);
        }

        [Test]
        public void CanVerifyThatNoDuplicateExistsDuringValidationProcess()
        {
            var contractor = new Contractor { Name = "Some unique name" };
            Assert.That(contractor.IsValid(ValidationContextFor(contractor)));
        }

        public void InitServiceLocatorInitializer()
        {
            IWindsorContainer container = new WindsorContainer();

            container.Register(
                Component
                    .For(typeof(IEntityDuplicateChecker))
                    .ImplementedBy(typeof(DuplicateCheckerStub))
                    .Named("duplicateChecker"));

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }

        [Test]
        public void MayNotUseValidatorWithEntityHavingDifferentIdType()
        {
            var invalidCombination = new ObjectWithStringIdAndValidatorForIntId { Name = "whatever" };

            Assert.Throws<PreconditionException>(() => invalidCombination.ValidationResults(ValidationContextFor(invalidCombination)));
        }

        [SetUp]
        public void SetUp()
        {
            //this.InitServiceLocatorInitializer();
            serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(sp => sp.GetService(typeof (IEntityDuplicateChecker)))
                .Returns(new DuplicateCheckerStub());
        }

        private ValidationContext ValidationContextFor(object instance)
        {
            return new ValidationContext(instance, serviceProviderMock.Object, null);
        }

        [HasUniqueDomainSignature]
        private class Contractor : Entity
        {
            [DomainSignature]
            public string Name { get; set; }
        }

        private class DuplicateCheckerStub : IEntityDuplicateChecker
        {
            public bool DoesDuplicateExistWithTypedIdOf<IdT>(IEntityWithTypedId<IdT> entity)
            {
                Check.Require(entity != null);

                if (entity as Contractor != null)
                {
                    var contractor = entity as Contractor;
                    return !string.IsNullOrEmpty(contractor.Name) && contractor.Name.ToLower() == "codai";
                }
                else if (entity as User != null)
                {
                    var user = entity as User;
                    return !string.IsNullOrEmpty(user.SSN) && user.SSN.ToLower() == "123-12-1234";
                }
                else if (entity as ObjectWithGuidId != null)
                {
                    var objectWithGuidId = entity as ObjectWithGuidId;
                    return !string.IsNullOrEmpty(objectWithGuidId.Name) && objectWithGuidId.Name.ToLower() == "codai";
                }

                // By default, simply return false for no duplicates found
                return false;
            }
        }

        [HasUniqueDomainSignatureWithGuidId]
        private class ObjectWithGuidId : EntityWithTypedId<Guid>
        {
            [DomainSignature]
            public string Name { get; set; }
        }

        [HasUniqueDomainSignature]
        private class ObjectWithStringIdAndValidatorForIntId : EntityWithTypedId<string>
        {
            [DomainSignature]
            public string Name { get; set; }
        }

        [HasUniqueDomainSignatureWithStringId]
        private class User : EntityWithTypedId<string>
        {
            [DomainSignature]
            public string SSN { get; set; }
        }
    }
}