using NUnit.Framework;
using SharpArch.Core.PersistenceSupport;
using Northwind.Core;
using Rhino.Mocks;
using System.Collections.Generic;
using Northwind.ApplicationServices;
using NUnit.Framework.SyntaxHelpers;

namespace Tests.Northwind.ApplicationServices
{
    [TestFixture]
    public class DashboardServiceTests
    {
        [Test]
        public void GetDashboardSummary() {
            DashboardService dashboardService = new DashboardService(CreateMockSupplierRepository());

            DashboardService.DashboardSummaryDto summary = dashboardService.GetDashboardSummary();

            Assert.That(summary.SuppliersCarryingMostProducts, Is.Not.Null);
            Assert.That(summary.SuppliersCarryingMostProducts.Count, Is.EqualTo(1));
            Assert.That(summary.SuppliersCarryingMostProducts[0].CompanyName, Is.EqualTo("Codai"));
            Assert.That(summary.SuppliersCarryingMostProducts[0].Products.Count, Is.EqualTo(2));

            Assert.That(summary.SuppliersCarryingFewestProducts, Is.Not.Null);
            Assert.That(summary.SuppliersCarryingFewestProducts.Count, Is.EqualTo(1));
            Assert.That(summary.SuppliersCarryingFewestProducts[0].CompanyName, Is.EqualTo("Acme"));
            Assert.That(summary.SuppliersCarryingFewestProducts[0].Products.Count, Is.EqualTo(1));
        }

        private IRepository<Supplier> CreateMockSupplierRepository() {
            MockRepository mocks = new MockRepository();

            IRepository<Supplier> mockedRepository = mocks.StrictMock<IRepository<Supplier>>();
            Expect.Call(mockedRepository.GetAll())
                .Return(CreateSuppliers());

            mocks.Replay(mockedRepository);

            return mockedRepository;
        }

        private IList<Supplier> CreateSuppliers() {
            IList<Supplier> suppliers = new List<Supplier>();

            suppliers.Add(CreateSupplierWithMostProducts());
            suppliers.Add(CreateSupplierWithFewestProducts());

            return suppliers;
        }

        private Supplier CreateSupplierWithMostProducts() {
            Supplier supplierWithMostProducts = new Supplier("Codai");
            supplierWithMostProducts.Products.Add(
                new Product() {
                    ProductName = "Training"
                });
            supplierWithMostProducts.Products.Add(
                new Product() {
                    ProductName = "Consulting"
                });
            return supplierWithMostProducts;
        }

        private Supplier CreateSupplierWithFewestProducts() {
            Supplier supplierWithMostProducts = new Supplier("Acme");
            supplierWithMostProducts.Products.Add(
                new Product() {
                    ProductName = "Whatever"
                });
            return supplierWithMostProducts;
        }
    }
}
