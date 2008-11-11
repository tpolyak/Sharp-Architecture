using System;
using NHibernate.Validator.Engine;

namespace SharpArch.Core
{
    /// <summary>
    /// Provides a simple means to develop your own base domain object
    /// </summary>
    public interface IDomainObject
    {
        bool IsValid();
        InvalidValue[] ValidationMessages { get; }
    }
}
