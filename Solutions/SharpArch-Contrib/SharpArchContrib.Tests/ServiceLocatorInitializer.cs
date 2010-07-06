using System;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using SharpArchContrib.Castle.CastleWindsor;
using SharpArchContrib.Data.NHibernate;
using Tests.NHibernateTests;
using Tests.SharpArchContrib.Castle.Logging;
using Tests.SharpArchContrib.Castle.NHibernate;

namespace Tests {
    public static class ServiceLocatorInitializer {
        public static void Init() {
            Init(typeof(NHibernateTransactionManager));
        }

        public static void Init(Type transactionManagerType) {
            IWindsorContainer container = new WindsorContainer();
            ComponentRegistrar.AddComponentsTo(container, transactionManagerType);
            RegisterTestServices(container);
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }

        private static void RegisterTestServices(IWindsorContainer container) {
            container.AddComponent("LogTestClass", typeof(ILogTestClass), typeof(LogTestClass));
            container.AddComponent("SystemTransactionTestProvider", typeof(ITransactionTestProvider),
                                   typeof(SystemTransactionTestProvider));
            container.AddComponent("NHibernateTransactionTestProvider", typeof(ITransactionTestProvider),
                                   typeof(NHibernateTransactionTestProvider));
            container.AddComponent("SystemUnitOfWorkTestProvider", typeof(ITransactionTestProvider),
                                   typeof(SystemUnitOfWorkTestProvider));
            container.AddComponent("NHibernateUnitOfWorkTestProvider", typeof(ITransactionTestProvider),
                                   typeof(NHibernateUnitOfWorkTestProvider));
            container.AddComponent("ExceptionHandlerTestClass", typeof(IExceptionHandlerTestClass),
                                   typeof(ExceptionHandlerTestClass));
        }
    }
}