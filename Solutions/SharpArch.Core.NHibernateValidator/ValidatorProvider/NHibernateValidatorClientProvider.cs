namespace SharpArch.Core.NHibernateValidator.ValidatorProvider
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using NHibernate.Validator.Constraints;
    using NHibernate.Validator.Engine;

    using RangeAttribute = NHibernate.Validator.Constraints.RangeAttribute;

    public class NHibernateValidatorClientProvider : AssociatedValidatorProvider
    {
        #region Constants and Fields

        private readonly RuleEmitterList<IRuleArgs> ruleEmitters;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// ctor: Hook up the mappings between your attributes and model client validation rules
        /// </summary>
        public NHibernateValidatorClientProvider()
        {
            this.ruleEmitters = new RuleEmitterList<IRuleArgs>();

            this.ruleEmitters.AddSingle<NotNullNotEmptyAttribute>(x => new ModelClientValidationRequiredRule(x.Message));
            this.ruleEmitters.AddSingle<NotEmptyAttribute>(x => new ModelClientValidationRequiredRule(x.Message));
            this.ruleEmitters.AddSingle<NotNullAttribute>(x => new ModelClientValidationRequiredRule(x.Message));

            this.ruleEmitters.AddSingle<LengthAttribute>(
                x => new ModelClientValidationStringLengthRule(x.Message, x.Min, x.Max));

            this.ruleEmitters.AddSingle<MinAttribute>(x => new ModelClientValidationRangeRule(x.Message, x.Value, null));
            this.ruleEmitters.AddSingle<MaxAttribute>(x => new ModelClientValidationRangeRule(x.Message, null, x.Value));

            this.ruleEmitters.AddSingle<RangeAttribute>(x => new ModelClientValidationRangeRule(x.Message, x.Min, x.Max));

            this.ruleEmitters.AddSingle<PatternAttribute>(x => new ModelClientValidationRegexRule(x.Message, x.Regex));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the validators for the given class metadata. This gets called for each property.
        /// </summary>
        /// <returns>Yield returns client validator instances with a list of rules for the current property</returns>
        protected override IEnumerable<ModelValidator> GetValidators(
            ModelMetadata metadata, ControllerContext context, IEnumerable<Attribute> attributes)
        {
            if (metadata.ContainerType == null)
            {
                yield break; // Break if there is no metadata container
            }

            ValidatorEngine engine = ValidatorEngineFactory.ValidatorEngine;

            IClassValidator validator = engine.GetClassValidator(metadata.ContainerType);
            IEnumerable<IRuleArgs> constraints =
                validator.GetMemberConstraints(metadata.PropertyName).OfType<IRuleArgs>();

            var rules = new List<ModelClientValidationRule>();

            foreach (IRuleArgs constraint in constraints)
            {
                // for each constraint, emit the rules for that constraint
                foreach (ModelClientValidationRule validationRule in this.ruleEmitters.EmitRules(constraint))
                {
                    validationRule.ErrorMessage = constraint.Message;
                        
                        // Temporarily give validation rule the error message provided by the validator
                    validationRule.ErrorMessage = this.MessageOrDefault(validationRule, metadata.PropertyName);
                        
                        // Get a true error message if not provided
                    rules.Add(validationRule);
                }
            }

            yield return new NHibernateValidatorClientValidator(metadata, context, rules);
        }

        protected string MessageOrDefault(ModelClientValidationRule rule, string propertyName)
        {
            // We don't want to display the default {validator.*} messages
            if ((rule.ErrorMessage != null) && !rule.ErrorMessage.StartsWith("{validator."))
            {
                return rule.ErrorMessage;
            }

            switch (rule.ValidationType)
            {
                case "stringLength":
                    var maxLength = (int)rule.ValidationParameters["maximumLength"];
                    return new StringLengthAttribute(maxLength).FormatErrorMessage(propertyName);
                case "required":
                    return new RequiredAttribute().FormatErrorMessage(propertyName);
                case "range":
                    double min = Convert.ToDouble(rule.ValidationParameters["minimum"]);
                    double max = Convert.ToDouble(rule.ValidationParameters["maximum"]);
                    return
                        new System.ComponentModel.DataAnnotations.RangeAttribute(min, max).FormatErrorMessage(
                            propertyName);
                case "regularExpression":
                    var pattern = (string)rule.ValidationParameters["pattern"];
                    return new RegularExpressionAttribute(pattern).FormatErrorMessage(propertyName);
                default:
                    throw new NotSupportedException(
                        "Only stringLength, Required, Range and RegularExpression validators are supported for generic error messages. Add a custom error message or choose another validator type");
            }
        }

        #endregion
    }
}