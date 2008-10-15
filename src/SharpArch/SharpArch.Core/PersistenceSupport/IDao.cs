using System.Collections.Generic;

namespace SharpArch.Core.PersistenceSupport
{
    /// <summary>
    /// Since nearly all of the domain objects you create will have a type of int ID, this 
    /// most freqently used base IDao leverages this assumption.  If you want a persistent 
    /// object with a type other than int, such as string, then use 
    /// <see cref="IDaoWithTypedId{T, IdT}" />.
    /// </summary>
    public interface IDao<T> : IDaoWithTypedId<T, int> { }

    public interface IDaoWithTypedId<T, IdT>
    {
        T Get(IdT id);
        T Get(IdT id, Enums.LockMode lockMode);
        T Load(IdT id);
        T Load(IdT id, Enums.LockMode lockMode);
        List<T> LoadAll();
        List<T> GetByExample(T exampleInstance, params string[] propertiesToExclude);
        T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude);
        T Save(T entity);
        T Update(T entity);
        T SaveOrUpdate(T entity);
        void Delete(T entity);
        void Evict(T entity);
        void CommitChanges();
    }
}
