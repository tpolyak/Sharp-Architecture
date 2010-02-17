using System.Collections.Generic;
using System.Reflection;

namespace SharpArch.Core.DomainModel
{
    /// <summary>
    /// This serves as a base interface for <see cref="EntityWithTypedId"/> and 
    /// <see cref="Entity"/>. Also provides a simple means to develop your own base entity.
    /// </summary>
    public interface IEntityWithTypedId<IdT>
    {
        IdT Id { get; }
        bool IsTransient();
        IEnumerable<PropertyInfo> GetSignatureProperties();
    }
}
