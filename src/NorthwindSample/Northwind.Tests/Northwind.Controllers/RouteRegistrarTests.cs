using NUnit.Framework;
using Northwind.Controllers;
using MvcContrib.TestHelper;
using System.Web.Routing;

namespace Tests.Northwind.Controllers
{
    [TestFixture]
    public class RouteRegistrarTests
    {
        [SetUp]
        public void SetUp() {
            RouteRegistrar.RegisterRoutesTo(RouteTable.Routes);
        }

        [Test]
        public void CanVerifyRouteMaps() {
            "~/".Route().ShouldMapTo<HomeController>(x => x.Index());
        }
    }
}
