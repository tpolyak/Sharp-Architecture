namespace SharpArch.Domain.Events
{
    /// <summary>
    ///     Defines the public members of a class that handles an event of the specified type.
    /// </summary>
    /// <typeparam name="T">The event type.</typeparam>
    public interface IHandles<in T> where T : IDomainEvent
    {
        /// <summary>
        ///     Handles the event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        void Handle(T args);
    }
}
