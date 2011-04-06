namespace SharpArch.Domain.PersistenceSupport
{
    using System.Collections.Generic;

    public interface IQuery<T>
    {
        IList<T> ExecuteQuery();
    }
}