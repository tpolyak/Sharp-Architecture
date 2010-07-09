namespace SharpArch.Core.NHibernateValidator.ValidatorProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using NHibernate.Validator.Constraints;
    using NHibernate.Validator.Engine;

    /// <summary>
    ///     Server side validator provider for NHVal
    /// </summary>
    public class NHibernateValidatorProvider : AssociatedValidatorProvider
    {
        private readonly RuleEmitterList<IRuleArgs> ruleEmitters;

        /// <summary>
        ///     ctor: Hook up the mappings between your attributes and model client validation rules
        /// </summary>
        public NHibernateValidatorProvider()
        {
            this.ruleEmitters = new RuleEmitterList<IRuleArgs>();

            this.ruleEmitters.AddSingle<NotNullNotEmptyAttribute>(
                x => new ModelClientValidationRequiredRule(x.Message));
            this.ruleEmitters.AddSingle<NotEmptyAttribute>(x => new ModelClientValidationRequiredRule(x.Message));
            this.ruleEmitters.AddSingle<NotNullAttribute>(x => new ModelClientValidationRequiredRule(x.Message));

            this.ruleEmitters.AddSingle<LengthAttribute>(
                x => new ModelClientValidationStringLengthRule(x.Message, x.Min, x.Max));

            this.ruleEmitters.AddSingle<MinAttribute>(
                x => new ModelClientValidationRangeRule(x.Message, x.Value, null));
            this.ruleEmitters.AddSingle<MaxAttribute>(
                x => new ModelClientValidationRangeRule(x.Message, null, x.Value));

            this.ruleEmitters.AddSingle<RangeAttribute>(
                x => new ModelClientValidationRangeRule(x.Message, x.Min, x.Max));

            this.ruleEmitters.AddSingle<PatternAttribute>(x => new ModelClientValidationRegexRule(x.Message, x.Regex));
        }

        protected override IEnumerable<ModelValidator> GetValidators(
            ModelMetadata metadata, ControllerContext context, IEnumerable<Attribute> attributes)
        {
            var validationEngine = ValidatorEngineFactory.ValidatorEngine;

            var classValidator = validationEngine.GetClassValidator(metadata.ModelType);

            if (classValidator != null)
            {
                yield return new NHibernateValidatorModelValidator(metadata, context, classValidator);
            }

            if (metadata.ContainerType == null)
            {
                yield break; // Break if there is no metadata container
            }

            var validator = validationEngine.GetClassValidator(metadata.ContainerType);
            if (validator == null)
            {
                yield break;
            }

            var constraints = validator.GetMemberConstraints(metadata.PropertyName).OfType<IRuleArgs>();

            var rules = new List<ModelClientValidationRule>();

            foreach (var constraint in constraints)
            {
                // for each constraint, emit the rules for that constraint
                foreach (var validationRule in this.ruleEmitters.EmitRules(constraint))
                {
                    if (validationRule != null)
                    {
                        validationRule.ErrorMessage = constraint.Message; // TODO: If specified
                    }

                    rules.Add(validationRule);
                }
            }

            if (rules.Count == 0)
            {
                yield break;
            }

            yield return new NHibernateValidatorClientValidator(metadata, context, rules);
        }
    }
}