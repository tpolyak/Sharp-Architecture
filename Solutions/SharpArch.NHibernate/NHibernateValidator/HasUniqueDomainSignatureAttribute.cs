namespace SharpArch.NHibernate.NHibernateValidator
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Provides a class level validator for determining if the entity has a unique domain signature
    ///     when compared with other entries in the database.
    ///     Due to the fact that .NET does not support generic attributes, this only works for entity
    ///     types having an Id of type int.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class HasUniqueDomainSignatureAttribute : HasUniqueDomainSignatureAttributeBase
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return DoValidate<int>(value, validationContext);
        }
    }
}
