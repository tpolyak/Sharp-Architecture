using Northwind.Core.DataInterfaces;
using NUnit.Framework;
using Northwind.Core;
using Rhino.Mocks;
using System.Collections.Generic;
using Northwind.ApplicationServices;
using SharpArch.Testing.NUnit;

namespace Tests.Northwind.ApplicationServices
{
    [TestFixture]
    public class DashboardServiceTests
    {
        [Test]
        public void CanGetDashboardSummary() {
            DashboardService dashboardService = new DashboardService(CreateMockSupplierRepository());

            DashboardService.DashboardSummaryDto summary = dashboardService.GetDashboardSummary();

            summary.SuppliersCarryingMostProducts.ShouldNotBeNull();
            summary.SuppliersCarryingMostProducts.Count.ShouldEqual(1);
            summary.SuppliersCarryingMostProducts[0].CompanyName.ShouldEqual("Codai");
            summary.SuppliersCarryingMostProducts[0].Products.Count.ShouldEqual(2);

            summary.SuppliersCarryingFewestProducts.ShouldNotBeNull();
            summary.SuppliersCarryingFewestProducts.Count.ShouldEqual(1);
            summary.SuppliersCarryingFewestProducts[0].CompanyName.ShouldEqual("Acme");
            summary.SuppliersCarryingFewestProducts[0].Products.Count.ShouldEqual(1);
        }

        private ISupplierRepository CreateMockSupplierRepository() {

            ISupplierRepository repository = MockRepository.GenerateMock<ISupplierRepository>( );
            repository.Expect(r => r.GetAll()).Return(CreateSuppliers());
            return repository;
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
