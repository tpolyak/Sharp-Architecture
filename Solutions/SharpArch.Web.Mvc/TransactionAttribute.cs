namespace SharpArch.Web.Mvc
{
    using System.Web.Mvc;

    using SharpArch.Domain;
    using SharpArch.Domain.PersistenceSupport;

    public class TransactionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the databse context
        /// The value should be injected by the filter provider.
        /// </summary>
        public ITransactionManager TransactionManager { get; set; }

        public bool RollbackOnModelStateError { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (((filterContext.Exception != null) && filterContext.ExceptionHandled) ||
                this.ShouldRollback(filterContext))
            {
                this.TransactionManager.RollbackTransaction();
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Check.Require(this.TransactionManager != null, "TransactionManager was null, register an implementation of TransactionManager in the IoC container.");

            this.TransactionManager.BeginTransaction();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            
            if (((filterContext.Exception != null) && (!filterContext.ExceptionHandled)) ||
                this.ShouldRollback(filterContext))
            {
                this.TransactionManager.RollbackTransaction();
            }
            else
            {
                this.TransactionManager.CommitTransaction();
            }
        }

        private bool ShouldRollback(ControllerContext filterContext)
        {
            return this.RollbackOnModelStateError && !filterContext.Controller.ViewData.ModelState.IsValid;
        }
    }
}