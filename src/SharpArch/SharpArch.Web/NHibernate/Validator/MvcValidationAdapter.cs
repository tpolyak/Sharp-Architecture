using NHibernate.Validator.Engine;
using System.Web.Mvc;
using SharpArch.Core;
using System;

namespace SharpArch.Web.NHibernate.Validator
{
    public class MvcValidationAdapter
    {
        public static ModelStateDictionary TransferValidationMessagesTo(ModelStateDictionary modelStateDictionary, 
            InvalidValue[] invalidValues) {
            Check.Require(modelStateDictionary != null, "modelStateDictionary may not be null");
            Check.Require(invalidValues != null, "invalidValues may not be null");

            foreach (InvalidValue invalidValue in invalidValues) {
                Check.Require(invalidValue.BeanClass != null, "The invalidValue.BeanClass property was null; " +
                    "perhaps you were creating your own InvalidValue items for a unit test and forgot to pass " +
                    "in the class Type?");

                modelStateDictionary.AddModelError(invalidValue.BeanClass.Name + "." +
                    invalidValue.PropertyName, invalidValue.Message);
            }

            return modelStateDictionary;
        }
    }
}
