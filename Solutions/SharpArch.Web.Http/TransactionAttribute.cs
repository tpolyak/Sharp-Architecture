namespace SharpArch.Web.Http
{
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using Domain;
    using Domain.PersistenceSupport;

    /// <summary>
    /// An attribute that implies a transaction.
    /// </summary>
    public class TransactionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the databse context
        /// The value should be injected by the filter provider.
        /// </summary>
        public ITransactionManager TransactionManager { get; set; }

        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Check.Require(this.TransactionManager != null, "TransactionManager was null, register an implementation of TransactionManager in the IoC container.");

            base.OnActionExecuting(actionContext);

            this.TransactionManager.BeginTransaction();
        }

        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            if (actionExecutedContext.Exception != null)
            {
                this.TransactionManager.RollbackTransaction();
            }
            else
            {
                this.TransactionManager.CommitTransaction();
            }
        }
    }
}
