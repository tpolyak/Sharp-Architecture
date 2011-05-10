namespace SharpArch.Domain.Specifications
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class QuerySpecification<T> : ILinqSpecification<T>
    {
        public virtual Expression<Func<T, bool>> MatchingCriteria
        {
            get
            {
                return null;
            }
        }

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