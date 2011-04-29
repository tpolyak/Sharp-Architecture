namespace SharpArch.NHibernate.NHibernateValidator.ValidatorProvider
{
    using global::NHibernate.Validator.Cfg;
    using global::NHibernate.Validator.Engine;
    using global::NHibernate.Validator.Event;

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