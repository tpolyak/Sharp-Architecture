﻿namespace SharpArch.Domain.PersistenceSupport
{
    using DomainModel;
    using JetBrains.Annotations;

    /// <summary>
    ///     Defines the public members of a class that checks an entity for duplicates.
    /// </summary>
    [PublicAPI]
    public interface IEntityDuplicateChecker
    {
        /// <summary>Returns a value indicating whether a duplicate of the specified <paramref name="entity" /> exists.</summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException"><see paramref="entity" /> is null.</exception>
        /// <returns>
        ///     <c>true</c> if a duplicate exists, <c>false</c> otherwise.
        /// </returns>
        bool DoesDuplicateExistWithTypedIdOf(IEntity entity);
    }
}
