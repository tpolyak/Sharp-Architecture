namespace SharpArch.Domain.PersistenceSupport
{
    using System;

    /// <summary>
    ///     Defines the public members of a class that represents a database context which handles
    ///     application wide DB activities such as committing any pending changes, beginning a
    ///     transaction, rolling back a transaction, etc.
    /// </summary>
    /// <remarks>
    ///     Note that outside of <see cref="T:IDbContext.CommitChanges"/>, you shouldn't have to
    ///     invoke this object very often. If you're using the
    ///     <see cref="SharpArch.NHibernate.Web.Mvc.TransactionAttribute" /> on your controller actions,
    ///     then the transaction opening/committing will be taken care of for you.
    /// </remarks>
    public interface IDbContext
    {
        /// <summary>
        ///     Begins the transaction.
        /// </summary>
        /// <returns>The transaction instance.</returns>
        IDisposable BeginTransaction();

        /// <summary>
        ///     Commits the changes.
        /// </summary>
        void CommitChanges();

        /// <summary>
        ///     Commits the transaction, saving all changes.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        ///     Rolls the transaction back, discarding any changes.
        /// </summary>
        void RollbackTransaction();
    }
}