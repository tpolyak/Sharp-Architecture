namespace SharpArch.NHibernate.Web.Mvc
{
    using System;
    using System.Web.Mvc;

    public class TransactionAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///     Optionally holds the factory key to be used when beginning/committing a transaction
        /// </summary>
        private readonly string factoryKey;

        /// <summary>
        ///     When used, assumes the <see cref = "factoryKey" /> to be NHibernateSession.DefaultFactoryKey
        /// </summary>
        public TransactionAttribute()
        {
        }

        /// <summary>
        ///     Overrides the default <see cref = "factoryKey" /> with a specific factory key
        /// </summary>
        public TransactionAttribute(string factoryKey)
        {
            this.factoryKey = factoryKey;
        }

        public bool RollbackOnModelStateError { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var effectiveFactoryKey = this.GetEffectiveFactoryKey();

            var currentTransaction = NHibernateSession.CurrentFor(effectiveFactoryKey).Transaction;

            if (currentTransaction.IsActive)
            {
                if (((filterContext.Exception != null) && filterContext.ExceptionHandled) ||
                    this.ShouldRollback(filterContext))
                {
                    currentTransaction.Rollback();
                }
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            NHibernateSession.CurrentFor(this.GetEffectiveFactoryKey()).BeginTransaction();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var effectiveFactoryKey = this.GetEffectiveFactoryKey();

            var currentTransaction = NHibernateSession.CurrentFor(effectiveFactoryKey).Transaction;

            base.OnResultExecuted(filterContext);
            try
            {
                if (currentTransaction.IsActive)
                {
                    if (((filterContext.Exception != null) && (!filterContext.ExceptionHandled)) ||
                        this.ShouldRollback(filterContext))
                    {
                        currentTransaction.Rollback();
                    }
                    else
                    {
                        currentTransaction.Commit();
                    }
                }
            }
            finally
            {
                currentTransaction.Dispose();
            }
        }

        private string GetEffectiveFactoryKey()
        {
            return String.IsNullOrEmpty(factoryKey) ? SessionFactoryKeyHelper.GetKey() : factoryKey;
        }

        private bool ShouldRollback(ControllerContext filterContext)
        {
            return this.RollbackOnModelStateError && !filterContext.Controller.ViewData.ModelState.IsValid;
        }
    }
}