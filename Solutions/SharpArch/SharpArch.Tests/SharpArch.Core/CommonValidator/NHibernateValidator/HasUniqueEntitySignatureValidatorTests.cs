namespace Tests.SharpArch.Core.CommonValidator.NHibernateValidator
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Castle.Windsor;

    using CommonServiceLocator.WindsorAdapter;

    using global::SharpArch.Core;
    using global::SharpArch.Core.CommonValidator;
    using global::SharpArch.Core.DomainModel;
    using global::SharpArch.Core.NHibernateValidator;
    using global::SharpArch.Core.NHibernateValidator.CommonValidatorAdapter;
    using global::SharpArch.Core.PersistenceSupport;

    using Microsoft.Practices.ServiceLocation;

    using NUnit.Framework;

    [TestFixture]
    public class HasUniqueObjectSignatureValidatorTests
    {
        #region Public Methods

        [Test]
        public void CanVerifyThatDuplicateExistsDuringValidationProcess()
        {
            var contractor = new Contractor { Name = "Codai" };
            IEnumerable<IValidationResult> invalidValues = contractor.ValidationResults();

            Assert.That(contractor.IsValid(), Is.False);

            foreach (IValidationResult invalidValue in invalidValues)
            {
                Debug.WriteLine(invalidValue.Message);
            }
        }

        [Test]
        public void CanVerifyThatDuplicateExistsOfEntityWithGuidIdDuringValidationProcess()
        {
            var objectWithGuidId = new ObjectWithGuidId { Name = "codai" };
            Assert.That(objectWithGuidId.IsValid(), Is.False);

            objectWithGuidId = new ObjectWithGuidId { Name = "whatever" };
            Assert.That(objectWithGuidId.IsValid(), Is.True);
        }

        [Test]
        public void CanVerifyThatDuplicateExistsOfEntityWithStringIdDuringValidationProcess()
        {
            var user = new User { SSN = "123-12-1234" };
            Assert.That(user.IsValid(), Is.False);
        }

        [Test]
        public void CanVerifyThatNoDuplicateExistsDuringValidationProcess()
        {
            var contractor = new Contractor { Name = "Some unique name" };
            Assert.That(contractor.IsValid());
        }

        public void InitServiceLocatorInitializer()
        {
            IWindsorContainer container = new WindsorContainer();

            container.AddComponent("duplicateChecker", typeof(IEntityDuplicateChecker), typeof(DuplicateCheckerStub));
            container.AddComponent("validator", typeof(IValidator), typeof(Validator));

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }

        [Test]
        public void MayNotUseValidatorWithEntityHavingDifferentIdType()
        {
            var invalidCombination = new ObjectWithStringIdAndValidatorForIntId { Name = "whatever" };

            Assert.Throws<PreconditionException>(() => invalidCombination.ValidationResults());
        }

        [SetUp]
        public void SetUp()
        {
            this.InitServiceLocatorInitializer();
        }

        #endregion

        [HasUniqueDomainSignature]
        private class Contractor : Entity
        {
            #region Properties

            [DomainSignature]
            public string Name { get; set; }

            #endregion
        }

        private class DuplicateCheckerStub : IEntityDuplicateChecker
        {
            #region Implemented Interfaces

            #region IEntityDuplicateChecker

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

            #endregion

            #endregion
        }

        [HasUniqueDomainSignatureWithGuidId]
        private class ObjectWithGuidId : EntityWithTypedId<Guid>
        {
            #region Properties

            [DomainSignature]
            public string Name { get; set; }

            #endregion
        }

        [HasUniqueDomainSignature]
        private class ObjectWithStringIdAndValidatorForIntId : EntityWithTypedId<string>
        {
            #region Properties

            [DomainSignature]
            public string Name { get; set; }

            #endregion
        }

        [HasUniqueDomainSignatureWithStringId]
        private class User : EntityWithTypedId<string>
        {
            #region Properties

            [DomainSignature]
            public string SSN { get; set; }

            #endregion
        }
    }
}