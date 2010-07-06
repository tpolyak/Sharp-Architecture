using NHibernate.Validator.Engine;
using NHibernate.Validator.Event;

namespace SharpArch.Core.NHibernateValidator.ValidatorProvider
{
    internal class ValidatorEngineFactory
    {
        public static ValidatorEngine ValidatorEngine
        {
            get
            {
                if (NHibernate.Validator.Cfg.Environment.SharedEngineProvider == null)
                {
                    NHibernate.Validator.Cfg.Environment.SharedEngineProvider = new NHibernateSharedEngineProvider();
                }

                return NHibernate.Validator.Cfg.Environment.SharedEngineProvider.GetEngine();
            }
        }
    }
}