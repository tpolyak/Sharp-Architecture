using System.Web.Mvc;
using System.Globalization;
using System;
using SharpArch.Core;
using System.Linq;
using System.ComponentModel;
using SharpArch.Core.PersistenceSupport;
using Microsoft.Practices.ServiceLocation;
using System.Reflection;
using SharpArch.Core.DomainModel;

namespace SharpArch.Web.ModelBinder
{
    internal class EntityValueProviderResult : ValueProviderResult
    {
        public EntityValueProviderResult(ValueProviderResult result, Type propertyType)
            : this(result.RawValue, result.AttemptedValue, result.Culture, propertyType) {
        }

        public EntityValueProviderResult(object rawValue, string attemptedValue, CultureInfo culture, Type propertyType)
            : base(rawValue, attemptedValue, culture) {
            Check.Require(propertyType != null, "propertyType may not be null");

            this.propertyType = propertyType;
        }

        public override object ConvertTo(Type type, CultureInfo culture) {
            Check.Require(type != null, "type may not be null");
            Check.Require(RawValue as string[] != null && (RawValue as string[]).Length == 1,
                "The EntityValueProviderResult can only work with a RawValue of type string[1]; the RawValue was " +
                (RawValue != null ? RawValue.ToString() : "null"));

            Type entityInterfaceType = propertyType.GetInterfaces()
                .First(interfaceType => interfaceType.IsGenericType
                    && interfaceType.GetGenericTypeDefinition() == typeof(IEntityWithTypedId<>));

            Type idType = entityInterfaceType.GetGenericArguments().First();
            string rawId = (RawValue as string[]).First();

            if (string.IsNullOrEmpty(rawId))
                return null;

            object typedId = Convert.ChangeType(rawId, idType);

            object entity = GetEntityFor(typedId);
            return entity;
        }

        private object GetEntityFor(object typedId) {
            object entityRepository = GetEntityRepository();

            return entityRepository.GetType()
                .InvokeMember("Get", BindingFlags.InvokeMethod, null, entityRepository, new[] { typedId });
        }

        public object GetEntityRepository() {
            Type genericRepositoryType = typeof(IRepository<>);
            Type concreteRepositoryType = genericRepositoryType.MakeGenericType(new Type[] { propertyType });

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

        private Type propertyType;
    }
}
