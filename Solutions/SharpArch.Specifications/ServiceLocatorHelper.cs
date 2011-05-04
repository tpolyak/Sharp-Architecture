namespace SharpArch.Features.Specifications
{
    #region Using Directives

    using Microsoft.Practices.ServiceLocation;

    using Rhino.Mocks;

    #endregion

    public static class ServiceLocatorHelper
    {
        private static IServiceLocator provider;

        public static void InitialiseServiceLocator()
        {
            provider = MockRepository.GenerateStub<IServiceLocator>();

            ServiceLocator.SetLocatorProvider(() => provider);
        }

        //public static Validator AddValidator()
        //{
        //    if (provider == null)
        //    {
        //        InitialiseServiceLocator();
        //    }

        //    var validator = new Validator();
        //    provider.Stub(p => p.GetInstance<IValidator>()).Return(validator);
        //    provider.Stub(p => p.GetService(typeof(IValidator))).Return(validator);

        //    return validator;
        //}

        public static T AddToServiceLocator<T>(this T o)
        {
            if (provider == null)
            {
                InitialiseServiceLocator();
            }

            provider.Stub(p => p.GetInstance<T>()).Return(o);
            provider.Stub(p => p.GetService(typeof(T))).Return(o);
            return o;
        }

        public static void Reset()
        {
            provider = null;
        }
    }
}