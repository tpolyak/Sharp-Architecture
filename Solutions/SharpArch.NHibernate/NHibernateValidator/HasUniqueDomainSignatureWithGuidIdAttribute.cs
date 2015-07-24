namespace SharpArch.NHibernate.NHibernateValidator
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class HasUniqueDomainSignatureWithGuidIdAttribute : HasUniqueDomainSignatureAttributeBase
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return DoValidate<Guid>(value, validationContext);
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class HasUniqueDomainSignatureWithBigIntIdAttribute : HasUniqueDomainSignatureAttributeBase
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return DoValidate<long>(value, validationContext);
        }
    }

}
