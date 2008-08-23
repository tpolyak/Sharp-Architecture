using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBase.Core.PersistenceSupport
{
    /// <summary>
    /// Since nearly all of the domain objects you create will have a type of int ID, this 
    /// most freqently used base IDao leverages this assumption.  If you want a persistent 
    /// object with a type other than int, such as string, then use 
    /// <see cref="IDaoWithTypedId" />.
    /// </summary>
    public interface IDao<T> : IDaoWithTypedId<T, int> { }

    public interface IDaoWithTypedId<T, IdT>
    {
        T Load(IdT id);
        T Load(IdT id, Enums.LockMode lockMode);
        List<T> LoadAll();
        T Save(T entity);
        T Update(T entity);
        T SaveOrUpdate(T entity);
        void Delete(T entity);
        void Evict(T entity);
        void CommitChanges();
    }
}
