using SharpArch.Core.PersistenceSupport;
using Northwind.Core;
using SharpArch.Core;
using Northwind.Core.DataInterfaces;
using System.Collections.Generic;

namespace Northwind.ApplicationServices
{
    /// <summary>
    /// This is an "application service" for coordinating the activities required by the view.
    /// Arguably, this service is so simplistic that I would lean towards putting this "logic"
    /// in the controller itself...but others would argue that ALL coordination with other services,
    /// such as repositories should be done within an application service, such as this.  The 
    /// nice thing about this service is that it encapsulates a bit of logic that is agnostic of
    /// the technology context; e.g., web services, ASP.NET MVC, WCF, console app, etc.  Consequently,
    /// it's easy to reuse without having any duplicated code amongst the various project types.
    /// </summary>
    public class DashboardService
    {
        /// <summary>
        /// Provided for mocking capabilities.
        /// </summary>
        protected DashboardService() { }

        /// <summary>
        /// Since DashboardService is registered as a component within Northwind.Web.CastleWindsor.ComponentRegistrar,
        /// its dependencies (e.g. supplierRepository) will automatically be injected when this service is
        /// injected into the constructor of another object (e.g., Northwind.Web.Controllers.DashboardController).
        /// </summary>
        public DashboardService(IRepository<Supplier> supplierRepository) {
            Check.Require(supplierRepository != null, "supplierRepository may not be null");

            this.supplierRepository = supplierRepository;
        }

        /// <summary>
        /// Uses the repository and domain layer to gather a few summary items for a dashboard view.
        /// Note that the method is a virtual method so that we can mock this class (which doesn't
        /// have an interface) in our unit testing.
        /// </summary>
        public virtual DashboardSummaryDto GetDashboardSummary() {
            DashboardSummaryDto dashboardSummaryDto = new DashboardSummaryDto();

            IList<Supplier> allSuppliers = supplierRepository.GetAll();

            // Arguably, the following two collection extension methods could be moved to 
            // ISupplierRepository, but since there's only 29 suppliers in the Northwind database, 
            // pushing this to the data layer isn't going to buy us any performance improvement.  
            // Consequently, IMO, I lean towards keeping such logic on the application side.
            // Furthermore, you should let a profiler inform you if have a bottle neck and then decide 
            // to optimize on the application or by pushing the logic and/or filtering to the database.
            dashboardSummaryDto.SuppliersCarryingMostProducts = allSuppliers.FindSuppliersCarryingMostProducts();
            dashboardSummaryDto.SuppliersCarryingFewestProducts = allSuppliers.FindSuppliersCarryingFewestProducts();

            return dashboardSummaryDto;
        }

        /// <summary>
        /// Arguably, this could go into a dedicated DTO layer.
        /// </summary>
        public class DashboardSummaryDto
        {
            public IList<Supplier> SuppliersCarryingMostProducts { get; set; }
            public IList<Supplier> SuppliersCarryingFewestProducts { get; set; }
        }

        private readonly IRepository<Supplier> supplierRepository;
    }
}
