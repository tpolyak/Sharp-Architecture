namespace SharpArch.Core.NHibernateValidator.ValidatorProvider
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    ///     Will help easily convert rules to model client validation rules.
    /// </summary>
    internal class RuleEmitterList<TInputBase>
    {
        private readonly List<RuleEmitter> ruleEmitters = new List<RuleEmitter>();

        public delegate IEnumerable<ModelClientValidationRule> RuleEmitter(TInputBase item);

        public void AddSingle<TSource>(Func<TSource, ModelClientValidationRule> emitter) where TSource : TInputBase
        {
            this.ruleEmitters.Add(
                x =>
                    {
                        if (x is TSource)
                        {
                            var rule = emitter((TSource)x);
                            return rule == null ? null : new[] { rule };
                        }
                        
                        return null;
                    });
        }

        public IEnumerable<ModelClientValidationRule> EmitRules(TInputBase item)
        {
            foreach (var emitter in this.ruleEmitters)
            {
                var emitterResult = emitter(item);
                if (emitterResult != null)
                {
                    return emitterResult;
                }
            }

            return new ModelClientValidationRule[] { }; // No matching emitter, so return an empty set of rules
        }
    }
}