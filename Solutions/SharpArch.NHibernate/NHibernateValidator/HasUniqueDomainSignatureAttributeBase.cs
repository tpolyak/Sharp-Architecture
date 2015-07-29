namespace SharpArch.NHibernate.NHibernateValidator
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using Domain;
    using Domain.DomainModel;
    using Domain.PersistenceSupport;

    public class HasUniqueDomainSignatureAttributeBase : ValidationAttribute
    {
        protected HasUniqueDomainSignatureAttributeBase()
            : base("Provided values matched an existing, duplicate entity")
        {
        }

        public override bool RequiresValidationContext
        {
            get { return true; }
        }

        protected ValidationResult DoValidate<TId>(object value, ValidationContext validationContext)
        {
            var entityToValidate = value as IEntityWithTypedId<TId>;
            Check.Require(
                entityToValidate != null, string.Format(CultureInfo.InvariantCulture,
                    "This validator must be used at the class level of an IDomainWithTypedId<{0}>. The type you provided was {1}",
                    typeof(TId), value.GetType())
                );

            var duplicateChecker = (IEntityDuplicateChecker) validationContext.GetService(typeof (IEntityDuplicateChecker));
            return duplicateChecker.DoesDuplicateExistWithTypedIdOf(entityToValidate)
                ? new ValidationResult(ErrorMessage)
                : ValidationResult.Success;
        }
    }
}