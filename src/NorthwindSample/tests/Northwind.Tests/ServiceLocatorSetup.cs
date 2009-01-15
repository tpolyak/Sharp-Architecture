using Castle.Windsor;
using SharpArch.Core.CommonValidator;
using CommonServiceLocator.WindsorAdapter;
using SharpArch.Core.NHibernateValidator.CommonValidatorAdapter;
using Microsoft.Practices.ServiceLocation;

namespace Tests
{
    public class ServiceLocatorSetup
    {
        public static void InitServiceLocator() {
            IWindsorContainer container = new WindsorContainer();
            container.AddComponent("validator", typeof(IValidator), typeof(Validator));
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }
    }
}
