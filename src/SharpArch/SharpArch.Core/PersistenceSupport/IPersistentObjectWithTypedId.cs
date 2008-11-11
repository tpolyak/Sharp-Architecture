namespace SharpArch.Core.PersistenceSupport
{
    /// <summary>
    /// Provides a simple means to develop your own base persistent object
    /// </summary>
    public interface IPersistentObjectWithTypedId<IdT>
    {
        IdT ID { get; }
        bool IsTransient();
    }
}
