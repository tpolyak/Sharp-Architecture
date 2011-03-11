namespace SharpArch.Domain.Specifications
{
    using System;
    using System.Linq.Expressions;

    public class AdHoc<T> : QuerySpecification<T>
    {
        private readonly Expression<Func<T, bool>> expression;

        public AdHoc(Expression<Func<T, bool>> expression)
        {
            this.expression = expression;
        }

        public override Expression<Func<T, bool>> MatchingCriteria
        {
            get
            {
                return this.expression;
            }
        }
    }
}