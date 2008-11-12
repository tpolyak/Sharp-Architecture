namespace SharpArch.Core.PersistenceSupport
{
    /// <summary>
    /// Provides a simple means to develop your own base persistent object.  This serves as a base 
    /// interface for <see cref="PersistentObjectWithTypedId"/> and <see cref="PersistentObject"/>.
    /// </summary>
    public interface IPersistentObjectWithTypedId<IdT>
    {
        IdT ID { get; }
        bool IsTransient();
    }
}
