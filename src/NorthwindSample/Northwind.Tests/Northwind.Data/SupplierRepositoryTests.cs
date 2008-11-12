using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Northwind.Core.DataInterfaces;
using Northwind.Core;
using NUnit.Framework.SyntaxHelpers;
using System.Diagnostics;
using SharpArch.Data.NHibernate;
using Northwind.Data;

namespace Tests.Northwind.Data
{
    [TestFixture]
    [Category("DB Tests")]
    public class SupplierRepositoryTests : RepositoryUnitTestsBase
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
                    Debug.WriteLine(" * Product name: " + product.Name);
                    Debug.WriteLine(" * Category name: " + product.Category.Name);
                }
            }
        }

        private ISupplierRepository supplierRepository = new SupplierRepository();
    }
}
