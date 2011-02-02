namespace SharpArch.Domain.NHibernateValidator.ValidatorProvider
{
    using NHibernate.Validator.Cfg;
    using NHibernate.Validator.Engine;
    using NHibernate.Validator.Event;

    internal class ValidatorEngineFactory
    {
        public static ValidatorEngine ValidatorEngine
        {
            get
            {
                if (Environment.SharedEngineProvider == null)
                {
                    Environment.SharedEngineProvider = new NHibernateSharedEngineProvider();
                }

                return Environment.SharedEngineProvider.GetEngine();
            }
        }
    }
}