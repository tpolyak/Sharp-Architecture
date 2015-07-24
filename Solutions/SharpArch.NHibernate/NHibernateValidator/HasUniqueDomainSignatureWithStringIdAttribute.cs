namespace SharpArch.NHibernate.NHibernateValidator
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class HasUniqueDomainSignatureWithStringIdAttribute : HasUniqueDomainSignatureAttributeBase
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return DoValidate<string>(value, validationContext);
        }
    }
}