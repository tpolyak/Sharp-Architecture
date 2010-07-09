namespace SharpArch.Core.NHibernateValidator
{
    using System;

    using NHibernate.Validator.Engine;

    using SharpArch.Core.DomainModel;
    using SharpArch.Core.PersistenceSupport;

    /// <summary>
    ///     Provides a class level validator for determining if the entity has a unique domain signature
    ///     when compared with other entries in the database.
    /// 
    ///     Due to the fact that .NET does not support generic attributes, this only works for entity 
    ///     types having an Id of type string.
    /// </summary>
    public class HasUniqueDomainSignatureWithGuidIdValidator : IValidator
    {
        public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
        {
            var entityToValidate = value as IEntityWithTypedId<Guid>;
            Check.Require(
                entityToValidate != null, 
                "This validator must be used at the class level of an " +
                "IdomainWithTypedId<string>. The type you provided was " + value.GetType() + ". " +
                "Other validators exist for various Id types. Please open an issue with S#arp Architecture " +
                "if you need a new Id type supported; you can make your own in the meantime.");

            var duplicateChecker = SafeServiceLocator<IEntityDuplicateChecker>.GetService();
            return !duplicateChecker.DoesDuplicateExistWithTypedIdOf(entityToValidate);
        }
    }
}