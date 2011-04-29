namespace SharpArch.NHibernate.NHibernateValidator
{
    using System;

    using global::NHibernate.Validator.Engine;

    [AttributeUsage(AttributeTargets.Class)]
    [ValidatorClass(typeof(HasUniqueDomainSignatureValidator))]
    public class HasUniqueDomainSignatureAttribute : Attribute, IRuleArgs
    {
        private string message = "Provided values matched an existing, duplicate entity";

        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
            }
        }
    }
}