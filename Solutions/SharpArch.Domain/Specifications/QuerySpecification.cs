namespace SharpArch.Domain.Specifications
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    ///     Serves as the base class for query specifications.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public abstract class QuerySpecification<T> : ILinqSpecification<T>
    {
        /// <summary>
        ///     Gets the matching criteria.
        /// </summary>
        public virtual Expression<Func<T, bool>> MatchingCriteria
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        ///     Returns the elements from the specified candidates that are satisfying the
        ///     specification.
        /// </summary>
        /// <param name="candidates">The candidates.</param>
        /// <returns>A list of satisfying elements.</returns>
        public virtual IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates)
        {
            if (this.MatchingCriteria != null)
            {
                return candidates.Where(this.MatchingCriteria).AsQueryable();
            }

            return candidates;
        }
    }
}