namespace SharpArch.Domain.Specifications
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public static class QuerySpecificationExtensions
    {
        public static QuerySpecification<T> And<T>(this QuerySpecification<T> specification1, QuerySpecification<T> specification2)
        {
            var adhocSpec1 = new AdHoc<T>(specification1.MatchingCriteria);
            var adhocSpec2 = new AdHoc<T>(specification2.MatchingCriteria);

            InvocationExpression invokedExpr = Expression.Invoke(adhocSpec2.MatchingCriteria, adhocSpec1.MatchingCriteria.Parameters.Cast<Expression>());
            Expression<Func<T, bool>> dynamicClause = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(adhocSpec1.MatchingCriteria.Body, invokedExpr), adhocSpec1.MatchingCriteria.Parameters);

            return new AdHoc<T>(dynamicClause);
        }

        public static QuerySpecification<T> Or<T>(this QuerySpecification<T> specification1, QuerySpecification<T> specification2)
        {
            var adhocSpec1 = new AdHoc<T>(specification1.MatchingCriteria);
            var adhocSpec2 = new AdHoc<T>(specification2.MatchingCriteria);

            InvocationExpression invokedExpr = Expression.Invoke(adhocSpec2.MatchingCriteria, adhocSpec1.MatchingCriteria.Parameters.Cast<Expression>());
            Expression<Func<T, bool>> dynamicClause = Expression.Lambda<Func<T, bool>>(Expression.OrElse(adhocSpec1.MatchingCriteria.Body, invokedExpr), adhocSpec1.MatchingCriteria.Parameters);

            return new AdHoc<T>(dynamicClause);
        }
    }
}