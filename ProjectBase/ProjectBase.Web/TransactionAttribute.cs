using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ProjectBase.Data.NHibernate;

namespace ProjectBase.Web
{
	public class TransactionAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext) {
			NHibernateSession.Current.BeginTransaction();
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext) {
			if(filterContext.Exception == null)
				NHibernateSession.Current.Transaction.Commit();
		}
	}
}
