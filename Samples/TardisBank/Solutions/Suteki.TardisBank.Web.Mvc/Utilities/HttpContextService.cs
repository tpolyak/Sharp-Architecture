namespace Suteki.TardisBank.Web.Mvc.Utilities
{
    using System.Web;

    using Suteki.TardisBank.Domain;
    using Suteki.TardisBank.Tasks;

    public class HttpContextService : IHttpContextService
    {
        public string UserName
        {
            get { return CurrentHttpContext.User.Identity.Name; }
        }

        public bool UserIsAuthenticated
        {
            get { return CurrentHttpContext.User.Identity.IsAuthenticated; }
        }

        static HttpContext CurrentHttpContext
        {
            get
            {
                var context = HttpContext.Current;
                if (context == null)
                {
                    throw new TardisBankException("HttpContext.Current is null");
                }
                return context;
            }
        }
    }
}