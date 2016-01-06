namespace SharpArch.Web.Http
{
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Domain;
    using Domain.PersistenceSupport;

    /// <summary>
    ///     An attribute that implies a transaction.
    /// </summary>
    public class TransactionAttribute : ActionFilterAttribute
    {

        /// <summary>
        ///     Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var transactionManager = GetTransactionManager(actionContext.Request);
            transactionManager.BeginTransaction();
            base.OnActionExecuting(actionContext);
        }

        /// <summary>
        ///     Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            var transactionManager = GetTransactionManager(actionExecutedContext.Request);

            if (actionExecutedContext.Exception != null)
            {
                transactionManager.RollbackTransaction();
            }
            else
            {
                transactionManager.CommitTransaction();
            }
        }

        private ITransactionManager GetTransactionManager(HttpRequestMessage request)
        {
            var transactionManager =
                (ITransactionManager) request.GetDependencyScope().GetService(typeof (ITransactionManager));
            Check.Require(transactionManager != null,
                "TransactionManager was null, register an implementation of TransactionManager in the IoC container.");

            return transactionManager;
        }
    }
}