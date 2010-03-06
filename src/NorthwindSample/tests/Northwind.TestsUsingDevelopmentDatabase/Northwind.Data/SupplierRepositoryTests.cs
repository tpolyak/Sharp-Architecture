using NUnit.Framework;
using Northwind.Core.DataInterfaces;
using Northwind.Core;
using System.Diagnostics;
using SharpArch.Data.NHibernate;
using Northwind.Data;
using SharpArch.Testing.NUnit.NHibernate;
using System.Collections.Generic;

namespace Tests.Northwind.Data
{
    [TestFixture]
    [Category("DB Tests")]
    public class SupplierRepositoryTests : DatabaseRepositoryTestsBase
    {
        [Test]
        public void CanLoadSuppliersByProductCategoryName() {
            List<Supplier> matchingSuppliers = supplierRepository.GetSuppliersBy("Seafood");

            Assert.That(matchingSuppliers.Count, Is.EqualTo(8));

            OutputSearchResults(matchingSuppliers);
        }

        private static void OutputSearchResults(List<Supplier> matchingSuppliers) {
            Debug.WriteLine("SupplierRepositoryTests.CanLoadSuppliersByProductCategoryName Results:");

            foreach (Supplier supplier in matchingSuppliers) {
                Debug.WriteLine("Company name: " + supplier.CompanyName);

                foreach (Product product in supplier.Products) {
                    Debug.WriteLine(" * Product name: " + product.ProductName);
                    Debug.WriteLine(" * Category name: " + product.Category.CategoryName);
                }
            }
        }

        private ISupplierRepository supplierRepository = new SupplierRepository();
    }
}
