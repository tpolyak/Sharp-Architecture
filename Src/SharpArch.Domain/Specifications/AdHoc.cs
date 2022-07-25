namespace SharpArch.Domain.Specifications
{
    using System;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    /// <summary>
    ///     An ad hoc query specification.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    [PublicAPI]
    public class AdHoc<T> : QuerySpecification<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AdHoc{T}" /> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public AdHoc(Expression<Func<T, bool>>? expression)
        {
            MatchingCriteria = expression;
        }

        /// <summary>
        ///     Gets the matching criteria.
        /// </summary>
        public override Expression<Func<T, bool>>? MatchingCriteria { get; }
    }
}
