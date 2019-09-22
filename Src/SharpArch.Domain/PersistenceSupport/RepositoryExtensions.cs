namespace SharpArch.Domain.PersistenceSupport
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    ///     Repository extension methods.
    /// </summary>
    [PublicAPI]
    public static class RepositoryExtensions
    {
        /// <summary>
        ///     Saves the specified object to the repository and evicts it from the session.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="repository" /> or <paramref name="entity"/> is <see langword="null" />.</exception>
        public static void SaveAndEvict<T, TId>([NotNull] this IRepositoryWithTypedId<T, TId> repository, [NotNull] T entity)
            where T: class
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            repository.Evict(repository.Save(entity));
        }
    }
}
