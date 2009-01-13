using NHibernate.Validator.Engine;
using System.Xml.Serialization;
using System;

namespace SharpArch.Core.DomainModel.WithNHibernateValidator
{
    public static class EntityValidation
    {
        public static bool IsValid(this Entity objectTovalidate) {
            return Validator.IsValid(objectTovalidate);
        }

        /// <summary>
        /// If this property isn't ignored by the XML serializer, a "missing constructor" exception 
        /// is thrown during serialization.
        /// </summary>
        [XmlIgnoreAttribute]
        public static InvalidValue[] ValidationMessages(this Entity objectTovalidate) {
            return Validator.Validate(objectTovalidate);
        }

        private static ValidatorEngine Validator {
            get {
                if (validator == null) {
                    validator = new ValidatorEngine();
                }

                return validator;
            }
        }

        [ThreadStatic]
        private static ValidatorEngine validator;
    }
}
