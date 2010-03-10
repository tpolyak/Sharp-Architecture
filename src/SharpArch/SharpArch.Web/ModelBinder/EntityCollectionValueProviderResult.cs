using System.Web.Mvc;
using System.Globalization;
using System;
using SharpArch.Core;
using System.Linq;
using System.Reflection;
using SharpArch.Core.DomainModel;

namespace SharpArch.Web.ModelBinder
{
    internal class EntityCollectionValueProviderResult : ValueProviderResult
    {
        public EntityCollectionValueProviderResult(ValueProviderResult result, Type collectionType)
            : this(result.RawValue, result.AttemptedValue, result.Culture, collectionType) {
        }

        /// <param name="collectionType">Is assumed to be a simple, generic colleciton of entity objects</param>
        public EntityCollectionValueProviderResult(object rawValue, string attemptedValue, CultureInfo culture, Type collectionType)
            : base(rawValue, attemptedValue, culture) {
            Check.Require(collectionType != null, "propertyType may not be null");

            this.collectionType = collectionType;
            this.collectionEntityType = collectionType.GetGenericArguments().First();
        }

        public override object ConvertTo(Type type, CultureInfo culture) {
            Check.Require(type != null, "type may not be null");
            Check.Require(RawValue as string[] != null,
                "The EntityCollectionValueProviderResult can only work with a RawValue of type string[]; the RawValue was " +
                (RawValue != null ? RawValue.ToString() : "null"));

            int countOfEntityIds = (RawValue as string[]).Length;
            Array entities = Array.CreateInstance(collectionEntityType, countOfEntityIds);

            Type entityInterfaceType = collectionEntityType.GetInterfaces()
                .First(interfaceType => interfaceType.IsGenericType
                    && interfaceType.GetGenericTypeDefinition() == typeof(IEntityWithTypedId<>));

            Type idType = entityInterfaceType.GetGenericArguments().First();

            for (int i = 0; i < countOfEntityIds; i++) {
                string rawId = (RawValue as string[])[i];

                if (string.IsNullOrEmpty(rawId))
                    return null;

                object typedId = 
                    (idType == typeof(Guid))
                        ? new Guid(rawId)
                        : Convert.ChangeType(rawId, idType);

                object entity = GetEntityFor(typedId, idType);
                entities.SetValue(entity, i);
            }

            return entities;
        }

        private object GetEntityFor(object typedId, Type idType) {
            object entityRepository = GenericRepositoryFactory.CreateEntityRepositoryFor(collectionEntityType, idType);

            return entityRepository.GetType()
                .InvokeMember("Get", BindingFlags.InvokeMethod, null, entityRepository, new[] { typedId });
        }

        private readonly Type collectionType;
        private readonly Type collectionEntityType;
    }
}
