using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;

namespace SharpArch.Core.NHibernateValidator.ValidatorProvider
{
    /// <summary>
    /// Server side validator provider for NHVal
    /// </summary>
    public partial class NHibernateValidatorProvider : AssociatedValidatorProvider
    {
        private readonly RuleEmitterList<IRuleArgs> _ruleEmitters;

        /// <summary>
        /// ctor: Hook up the mappings between your attributes and model client validation rules
        /// </summary>
        public NHibernateValidatorProvider()
        {
            _ruleEmitters = new RuleEmitterList<IRuleArgs>();

            _ruleEmitters.AddSingle<NotNullNotEmptyAttribute>(x => new ModelClientValidationRequiredRule(x.Message));
            _ruleEmitters.AddSingle<NotEmptyAttribute>(x => new ModelClientValidationRequiredRule(x.Message));
            _ruleEmitters.AddSingle<NotNullAttribute>(x => new ModelClientValidationRequiredRule(x.Message));

            _ruleEmitters.AddSingle<LengthAttribute>(
                x => new ModelClientValidationStringLengthRule(x.Message, x.Min, x.Max));

            _ruleEmitters.AddSingle<MinAttribute>(x => new ModelClientValidationRangeRule(x.Message, x.Value, null));
            _ruleEmitters.AddSingle<MaxAttribute>(x => new ModelClientValidationRangeRule(x.Message, null, x.Value));

            _ruleEmitters.AddSingle<RangeAttribute>(
                x => new ModelClientValidationRangeRule(x.Message, x.Min, x.Max));

            _ruleEmitters.AddSingle<PatternAttribute>(x => new ModelClientValidationRegexRule(x.Message, x.Regex));
        }
        
        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context, IEnumerable<System.Attribute> attributes)
        {
            var validationEngine = ValidatorEngineFactory.ValidatorEngine;

            var classValidator = validationEngine.GetClassValidator(metadata.ModelType);

            if (classValidator != null)
                yield return new NHibernateValidatorModelValidator(metadata, context, classValidator);


            if (metadata.ContainerType == null) yield break; //Break if there is no metadata container

            var validator = validationEngine.GetClassValidator(metadata.ContainerType);
            if (validator == null)
                yield break;

            var constraints = validator.GetMemberConstraints(metadata.PropertyName).OfType<IRuleArgs>();

            var rules = new List<ModelClientValidationRule>();

            foreach (var constraint in constraints) //for each constraint, emit the rules for that constraint
            {
                foreach (var validationRule in _ruleEmitters.EmitRules(constraint))
                {
                    if (validationRule != null)
                    {
                        validationRule.ErrorMessage = constraint.Message; //TODO: If specified
                    }

                    rules.Add(validationRule);
                }
            }

            if (rules.Count == 0)
                yield break;

            yield return new NHibernateValidatorClientValidator(metadata, context, rules);
        }

    }
}