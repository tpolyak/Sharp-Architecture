using Northwind.Web.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MvcContrib.TestHelper;
using SharpArch.Core.PersistenceSupport;
using Northwind.Core;
using Rhino.Mocks;
using NUnit.Framework.SyntaxHelpers;
using Northwind.Core.DataInterfaces;
using SharpArch.Testing;
using Northwind.ApplicationServices;

namespace Tests.Northwind.Web.Controllers
{
    [TestFixture]
    public class DashboardControllerTests
    {
        [Test]
        public void CanShowSummaryData() {
            DashboardController controller =
                new DashboardController(CreateMockDashboardServiceRepository());

            ViewResult result =
                controller.Index().AssertViewRendered().ForView("");

            Assert.That(result.ViewData, Is.Not.Null);
            Assert.That(result.ViewData.Model as DashboardService.DashboardSummaryDto, Is.Not.Null);

            // I could go on to test the internals of the summary DTO, but I don't really care
            // since here I'm mostly concerned the the controller is getting a response from the
            // application service and giving the summary DTO to the view.  The application service
            // unit tests verify that the service itself is returning valid data.
        }

        public DashboardService CreateMockDashboardServiceRepository() {
            MockRepository mocks = new MockRepository();

            DashboardService mockedService = mocks.StrictMock<DashboardService>();
            Expect.Call(mockedService.GetDashboardSummary())
                .Return(new DashboardService.DashboardSummaryDto());
            mocks.Replay(mockedService);

            return mockedService;
        }
    }
}
