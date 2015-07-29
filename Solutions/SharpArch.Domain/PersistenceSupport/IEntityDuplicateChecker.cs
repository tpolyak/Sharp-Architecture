namespace SharpArch.Domain.PersistenceSupport
{
    using DomainModel;

    /// <summary>
    ///     Defines the public members of a class that checks an entity for duplicates.
    /// </summary>
    public interface IEntityDuplicateChecker
    {
        /// <summary>
        ///     Returns a value indicating whether a duplicate of the specified entity exists.
        /// </summary>
        /// <typeparam name="TId">The type of the ID that identifies the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if a duplicate exists, <c>false</c> otherwise.</returns>
        bool DoesDuplicateExistWithTypedIdOf<TId>(IEntityWithTypedId<TId> entity);
    }
}