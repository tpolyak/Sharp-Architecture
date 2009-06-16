using NUnit.Framework;
using Microsoft.Practices.ServiceLocation;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using SharpArch.Core.PersistenceSupport.NHibernate;
using Rhino.Mocks;
using NHibernate.Validator.Engine;
using NHibernate.Validator;
using SharpArch.Core.DomainModel;
using SharpArch.Core;
using System.Diagnostics;
using System;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core.NHibernateValidator;
using System.Collections.Generic;
using SharpArch.Core.NHibernateValidator.CommonValidatorAdapter;
using IValidator = SharpArch.Core.CommonValidator.IValidator;
using SharpArch.Core.CommonValidator;

namespace Tests.SharpArch.Core.CommonValidator.NHibernateValidator
{
    [TestFixture]
    public class HasUniqueObjectSignatureValidatorTests
    {
        [SetUp]
        public void SetUp() {
            InitServiceLocatorInitializer();
        }

        public void InitServiceLocatorInitializer() {
            IWindsorContainer container = new WindsorContainer();
            container.AddComponent("duplicateChecker",
                typeof(IEntityDuplicateChecker), typeof(DuplicateCheckerStub));
            container.AddComponent("validator",
                typeof(IValidator), typeof(Validator));
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }

        [Test]
        public void CanVerifyThatDuplicateExistsDuringValidationProcess() {
            Contractor contractor = new Contractor() { Name = "Codai" };
            IEnumerable<IValidationResult> invalidValues = contractor.ValidationResults();

            Assert.That(contractor.IsValid(), Is.False);

            foreach (IValidationResult invalidValue in invalidValues) {
                Debug.WriteLine(invalidValue.Message);
            }
        }

        [Test]
        public void CanVerifyThatNoDuplicateExistsDuringValidationProcess() {
            Contractor contractor = new Contractor() { Name = "Some unique name" };
            Assert.That(contractor.IsValid());
        }

        [Test]
        public void CanVerifyThatDuplicateExistsOfEntityWithStringIdDuringValidationProcess() {
            User user = new User() { SSN = "123-12-1234" };
            Assert.That(user.IsValid(), Is.False);
        }

        [Test]
        public void CanVerifyThatDuplicateExistsOfEntityWithGuidIdDuringValidationProcess() {
            ObjectWithGuidId objectWithGuidId = new ObjectWithGuidId() { Name = "codai" };
            Assert.That(objectWithGuidId.IsValid(), Is.False);

            objectWithGuidId = new ObjectWithGuidId() { Name = "whatever" };
            Assert.That(objectWithGuidId.IsValid(), Is.True);
        }

        [Test]
        public void MayNotUseValidatorWithEntityHavingDifferentIdType() {
            ObjectWithStringIdAndValidatorForIntId invalidCombination = 
                new ObjectWithStringIdAndValidatorForIntId() { Name = "whatever" };

            Assert.Throws<PreconditionException>(
                () => invalidCombination.ValidationResults()
            );
        }

        private class DuplicateCheckerStub : IEntityDuplicateChecker
        {
            public bool DoesDuplicateExistWithTypedIdOf<IdT>(IEntityWithTypedId<IdT> entity) {
                Check.Require(entity != null);

                if (entity as Contractor != null) {
                    Contractor contractor = entity as Contractor;
                    return !string.IsNullOrEmpty(contractor.Name) && contractor.Name.ToLower() == "codai";
                }
                else if (entity as User != null) {
                    User user = entity as User;
                    return !string.IsNullOrEmpty(user.SSN) && user.SSN.ToLower() == "123-12-1234";
                }
                else if (entity as ObjectWithGuidId != null) {
                    ObjectWithGuidId objectWithGuidId = entity as ObjectWithGuidId;
                    return !string.IsNullOrEmpty(objectWithGuidId.Name) && objectWithGuidId.Name.ToLower() == "codai";
                }

                // By default, simply return false for no duplicates found
                return false;
            }
        }

        [HasUniqueDomainSignature]
        private class Contractor : Entity
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
    }
}
