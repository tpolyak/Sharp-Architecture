using System.Web.Mvc;
using SharpArch.Data.NHibernate;
using System;
using NHibernate;

namespace SharpArch.Web.NHibernate
{
	public class TransactionAttribute : ActionFilterAttribute
	{
        /// <summary>
        /// When used, assumes the <see cref="factoryKey" /> to be NHibernateSession.DefaultFactoryKey
        /// </summary>
        public TransactionAttribute() { }

        public bool RollbackOnModelStateError { get;  set; }

        /// <summary>
        /// Overrides the default <see cref="factoryKey" /> with a specific factory key
        /// </summary>
        public TransactionAttribute(string factoryKey) {
            this.factoryKey = factoryKey;
	    }

		public override void OnActionExecuting(ActionExecutingContext filterContext) {
            NHibernateSession.CurrentFor(GetEffectiveFactoryKey()).BeginTransaction();
		}

        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            string effectiveFactoryKey = GetEffectiveFactoryKey();

            ITransaction currentTransaction = 
                NHibernateSession.CurrentFor(effectiveFactoryKey).Transaction;
                
            if (currentTransaction.IsActive) {
                if (((filterContext.Exception != null) && (filterContext.ExceptionHandled)) || ShouldRollback(filterContext))
                {
                    currentTransaction.Rollback();
                }
            }
        }

	    public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            string effectiveFactoryKey = GetEffectiveFactoryKey();

            ITransaction currentTransaction =
                NHibernateSession.CurrentFor(effectiveFactoryKey).Transaction;
            
            base.OnResultExecuted(filterContext);
            try
            {
                if (currentTransaction.IsActive) {
                    if (((filterContext.Exception != null) && (!filterContext.ExceptionHandled)) || ShouldRollback(filterContext))
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

        private string GetEffectiveFactoryKey() {
            return String.IsNullOrEmpty(factoryKey)
                    ? NHibernateSession.DefaultFactoryKey
                    : factoryKey;
        }

        private bool ShouldRollback(ControllerContext filterContext)
        {
            return RollbackOnModelStateError && !filterContext.Controller.ViewData.ModelState.IsValid;
        }

        /// <summary>
        /// Optionally holds the factory key to be used when beginning/committing a transaction
        /// </summary>
        private string factoryKey;
	}
}
