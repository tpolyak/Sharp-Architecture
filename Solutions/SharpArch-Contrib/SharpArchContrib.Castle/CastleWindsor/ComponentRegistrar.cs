using System;
using Castle.Windsor;
using SharpArchContrib.Castle.Logging;
using SharpArchContrib.Castle.NHibernate;
using SharpArchContrib.Core;
using SharpArchContrib.Data.NHibernate;

namespace SharpArchContrib.Castle.CastleWindsor {
    public static class ComponentRegistrar {
        public static void AddComponentsTo(IWindsorContainer container) {
            AddComponentsTo(container, typeof(NHibernateTransactionManager));
        }

        public static void AddComponentsTo(IWindsorContainer container, Type transactionManagerType) {
            ParameterCheck.ParameterRequired(container, "container");

            if (!container.Kernel.HasComponent("LogInterceptor")) {
                Core.CastleWindsor.ComponentRegistrar.AddComponentsTo(container);
                Data.CastleWindsor.ComponentRegistrar.AddComponentsTo(container, transactionManagerType);
                container.AddFacility("LogFacility", new LogFacility());
                container.AddFacility("ExceptionHandlerFacility", new ExceptionHandlerFacility());
                container.AddFacility("TransactionFacility", new TransactionFacility());
                container.AddFacility("UnitOfWorkFacility", new UnitOfWorkFacility());
            }
        }
    }
}