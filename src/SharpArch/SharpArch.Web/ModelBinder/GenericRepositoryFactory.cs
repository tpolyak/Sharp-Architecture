using SharpArch.Core.PersistenceSupport;
using System;
using Microsoft.Practices.ServiceLocation;

namespace SharpArch.Web.ModelBinder
{
    internal class GenericRepositoryFactory
    {
        public static object CreateEntityRepositoryFor(Type entityType, Type idType) {
            Type genericRepositoryType = typeof(IRepositoryWithTypedId<,>);
            Type concreteRepositoryType =
                genericRepositoryType.MakeGenericType(new Type[] { entityType, idType });

            object repository;

            try {
                repository = ServiceLocator.Current.GetService(concreteRepositoryType);
            }
            catch (NullReferenceException) {
                throw new NullReferenceException("ServiceLocator has not been initialized; " +
                    "I was trying to retrieve " + concreteRepositoryType.ToString());
            }
            catch (ActivationException) {
                throw new ActivationException("The needed dependency of type " + concreteRepositoryType.Name +
                    " could not be located with the ServiceLocator. You'll need to register it with " +
                    "the Common Service Locator (CSL) via your IoC's CSL adapter.");
            }

            return repository;
        }
    }
}
