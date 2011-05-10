namespace SharpArch.NHibernate.NHibernateValidator
{
    using System.ComponentModel.DataAnnotations;

    using SharpArch.Domain;
    using SharpArch.Domain.DomainModel;
    using SharpArch.Domain.PersistenceSupport;

    public class HasUniqueDomainSignatureWithStringIdAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var entityToValidate = value as IEntityWithTypedId<string>;
            Check.Require(
                entityToValidate != null,
                "This validator must be used at the class level of an IDomainWithTypedId<string>. The type you provided was " + value.GetType());

            var duplicateChecker = SafeServiceLocator<IEntityDuplicateChecker>.GetService();
            return !duplicateChecker.DoesDuplicateExistWithTypedIdOf(entityToValidate);
        }

        public override string FormatErrorMessage(string name)
        {
            return "Provided values matched an existing, duplicate entity";
        }
    }
}