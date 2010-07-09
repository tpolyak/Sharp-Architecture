//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------

#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace SharpArch.Specifications.SharpArch.Core.NHibernateValidator
{
    using System;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using CommonServiceLocator.WindsorAdapter;

    using global::SharpArch.Core;
    using global::SharpArch.Core.CommonValidator;
    using global::SharpArch.Core.DomainModel;
    using global::SharpArch.Core.NHibernateValidator;
    using global::SharpArch.Core.PersistenceSupport;

    using Machine.Specifications;

    using Microsoft.Practices.ServiceLocation;

    using Rhino.Mocks;

    public abstract class specification_for_has_unique_domain_signature_validator
    {
        protected static IValidator validator;

        protected static IEntityDuplicateChecker entityDuplicateChecker;

        protected static string entityName;

        protected static string entitySSN;

        private Establish context_for_each = () =>
            {
                validator = MockRepository.GenerateMock<IValidator>();
                entityDuplicateChecker = MockRepository.GenerateStub<IEntityDuplicateChecker>();
                entitySSN = "111-22-3333";
                entityName = "codai";

                ServiceLocator.SetLocatorProvider(null);
            };

        protected static void CreateSut()
        {
            IWindsorContainer container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
            container.Register(Component.For<IEntityDuplicateChecker>().Instance(entityDuplicateChecker));
            container.Register(Component.For<IValidator>().Instance(validator));
        }
    }

    [Subject(typeof(HasUniqueDomainSignatureValidator))]
    public class when_a_duplicate_exists_of_entity_during_validation_process : specification_for_has_unique_domain_signature_validator
    {
        static Contractor contractor;

        static bool result;

        Establish context = () =>
            {
                result = false;
                contractor = new Contractor() { Name = entityName };
                validator.Stub(a => a.IsValid(contractor)).IgnoreArguments().Return(false);

                CreateSut();
            };

        Because of = () =>
            {
                result = contractor.IsValid(); 
            };

        It should_ask_the_validator_if_it_has_a_duplicate = () => validator.AssertWasCalled(a => a.IsValid(contractor));

        It should_return_false = () => result.ShouldBeFalse();
    }

    [Subject(typeof(HasUniqueDomainSignatureValidator))]
    public class when_duplicate_exists_of_entity_with_guid_id_during_validation_process : specification_for_has_unique_domain_signature_validator
    {
        static ObjectWithGuidId objectWithGuidId1;

        static bool result;

        Establish context = () =>
            {
                objectWithGuidId1 = new ObjectWithGuidId {Name = entityName};

                validator.Stub(a => a.IsValid(objectWithGuidId1)).IgnoreArguments().Return(false);

                CreateSut();
            };

        Because of = () => result = objectWithGuidId1.IsValid();

        It should_ask_the_validator_if_it_has_a_duplicate = () => validator.AssertWasCalled(a => a.IsValid(objectWithGuidId1));

        It should_return_false_for_the_duplicate_object = () => result.ShouldBeFalse();
    }

    [Subject(typeof(HasUniqueDomainSignatureValidator))]
    public class when_a_duplicate_exists_of_entity_with_string_id_during_validation_process : specification_for_has_unique_domain_signature_validator
    {
        static User user;

        static bool result;

        Establish context = () =>
            {
                user = new User { SSN = "123-12-1234" };
                validator.Stub(a => a.IsValid(user)).IgnoreArguments().Return(false);

                CreateSut();
            };

        Because of = () => result = user.IsValid();

        It should_ask_the_validator_if_it_has_a_duplicate = () => validator.AssertWasCalled(a => a.IsValid(user));

        It should_return_false_when_is_valid_is_called = () => result.ShouldBeFalse();
    }

    [Subject(typeof(HasUniqueDomainSignatureValidator))]
    public class when_no_duplicate_exists_during_validation_process : specification_for_has_unique_domain_signature_validator
    {
        static Contractor contractor;

        static bool result;

        Establish context = () =>
            { 
                result = false;
                contractor = new Contractor { Name = "the name" };
                validator.Stub(a => a.IsValid(contractor)).IgnoreArguments().Return(true);

                CreateSut();
            };

        Because of = () => result = contractor.IsValid();

        It should_ask_the_validator_if_it_has_a_duplicate = () => validator.AssertWasCalled(a => a.IsValid(contractor));

        It should_return_true_when_is_valid_is_called = () => result.ShouldBeTrue();
    }

    [Subject(typeof(HasUniqueDomainSignatureValidator))]
    public class when_wrong_validator_is_used_with_entity_having_different_id_type : specification_for_has_unique_domain_signature_validator
    {
        static ObjectWithStringIdAndValidatorForIntId entity;

        static Exception result;

        Establish context = () =>
            {
                entity = new ObjectWithStringIdAndValidatorForIntId { Name = "whatever" };
                validator.Stub(a => a.IsValid(entity)).IgnoreArguments().Throw(new PreconditionException());

                CreateSut();
            };

        Because of = () => result = Catch.Exception(() => entity.IsValid());

        It should_throw_a_precondition_exception = () => result.ShouldBeOfType(typeof(PreconditionException));
    }

    #region Private Methods

    [HasUniqueDomainSignature]
    class Contractor : Entity
    {
        #region Properties

        [DomainSignature]
        public string Name { get; set; }

        #endregion
    }

    class DuplicateCheckerStub : IEntityDuplicateChecker
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
    class ObjectWithGuidId : EntityWithTypedId<Guid>
    {
        #region Properties

        [DomainSignature]
        public string Name { get; set; }

        #endregion
    }

    [HasUniqueDomainSignature]
    class ObjectWithStringIdAndValidatorForIntId : EntityWithTypedId<string>
    {
        #region Properties

        [DomainSignature]
        public string Name { get; set; }

        #endregion
    }

    [HasUniqueDomainSignatureWithStringId]
    class User : EntityWithTypedId<string>
    {
        #region Properties

        [DomainSignature]
        public string SSN { get; set; }

        #endregion
    }

    #endregion

}