using System;
using System.Reflection;

namespace SharpArch.Web.ModelBinder
{
    internal static class ValueBinderHelper
    {
        internal static object GetEntityFor(Type collectionEntityType, object typedId, Type idType)
        {
            object entityRepository = GenericRepositoryFactory.CreateEntityRepositoryFor(collectionEntityType, idType);

            return entityRepository.GetType()
                .InvokeMember("Get", BindingFlags.InvokeMethod, null, entityRepository, new[] { typedId });
        }
    }
}
