using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SharpArch.Core.NHibernateValidator.ValidatorProvider
{
    /// <summary>
    /// Will help easily convert rules to model client validation rules.
    /// </summary>
    public class RuleEmitterList<TInputBase>
    {
        public delegate IEnumerable<ModelClientValidationRule> RuleEmitter(TInputBase item);

        private readonly List<RuleEmitter> _ruleEmitters = new List<RuleEmitter>();

        public void AddSingle<TSource>(Func<TSource, ModelClientValidationRule> emitter) where TSource : TInputBase
        {
            _ruleEmitters.Add(x =>
                                  {

                                      if (x is TSource)
                                      {
                                          ModelClientValidationRule rule = emitter((TSource)x);
                                          return rule == null ? null : new[] { rule };
                                      }
                                      else
                                      {
                                          return null;
                                      }
                                  });
        }

        public IEnumerable<ModelClientValidationRule> EmitRules(TInputBase item)
        {
            foreach (var emitter in _ruleEmitters)
            {
                var emitterResult = emitter(item);
                if (emitterResult != null)
                {
                    return emitterResult;
                }
            }

            return new ModelClientValidationRule[] { }; //No matching emitter, so return an empty set of rules
        }
    }
}