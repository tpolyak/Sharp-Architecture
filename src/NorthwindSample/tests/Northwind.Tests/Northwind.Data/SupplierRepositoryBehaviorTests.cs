using NUnit.Framework;
using Northwind.Core.DataInterfaces;
using Northwind.Core;
using System.Diagnostics;
using SharpArch.Data.NHibernate;
using Northwind.Data;
using SharpArch.Testing.NUnit;
using SharpArch.Testing.NUnit.NHibernate;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport;

namespace Tests.Northwind.Data
{
    [TestFixture]
    [Category("DB Tests")]
    public class WhenGettingSuppliersByCategory : RepositoryBehaviorSpecificationTestsBase
    {
        protected override void EstablishContext() {
            Category beverages = CreatePersistedCategory("Beverages");
            Category seafood = CreatePersistedCategory("Seafood");

            Supplier supplier = CreatePersistedSupplier("Acme");
            CreatePersistedProduct(supplier, beverages, "Coke");
            CreatePersistedProduct(supplier, beverages, "Sprite");

            // This one should match
            supplier = CreatePersistedSupplier("Anvil");
            CreatePersistedProduct(supplier, beverages, "Root Beer");
            CreatePersistedProduct(supplier, seafood, "Sushi");

            // This one should match
            supplier = CreatePersistedSupplier("Roadrunner");
            CreatePersistedProduct(supplier, seafood, "Swordfish");

            supplier = CreatePersistedSupplier("Coyote");
        }

        protected override void Act() {
            matchingSuppliers = supplierRepository.GetSuppliersBy("Seafood");
        }

        [Test]
        public void SuppliersShouldBeThoseHavingProductsWithinTheCategory() {
            matchingSuppliers.Count.ShouldEqual(2);
            matchingSuppliers[0].CompanyName.ShouldEqual("Anvil");
            matchingSuppliers[1].CompanyName.ShouldEqual("Roadrunner");

            OutputSearchResults(matchingSuppliers);
        }

        /// <summary>
        /// Simply here for debugging
        /// </summary>
        private void OutputSearchResults(List<Supplier> matchingSuppliers) {
            Debug.WriteLine("SupplierRepositoryTests.CanLoadSuppliersByProductCategoryName Results:");

            foreach (Supplier supplier in matchingSuppliers) {
                Debug.WriteLine("Company name: " + supplier.CompanyName);

                foreach (Product product in supplier.Products) {
                    Debug.WriteLine(" * Product name: " + product.ProductName);
                    Debug.WriteLine(" * Category name: " + product.Category.CategoryName);
                }
            }
        }

        private Supplier CreatePersistedSupplier(string supplierName) {
            Supplier supplier = new Supplier(supplierName);
            supplierRepository.SaveOrUpdate(supplier);
            FlushSessionAndEvict(supplier);

            return supplier;
        }

        private Category CreatePersistedCategory(string categoryName) {
            Category category = new Category(categoryName);
            categoryRepository.SaveOrUpdate(category);
            FlushSessionAndEvict(category);

            return category;
        }

        private Product CreatePersistedProduct(Supplier supplier, Category category, string name) {
            Product product = new Product(name, supplier);
            product.Category = category;
            productRepository.SaveOrUpdate(product);
            FlushSessionAndEvict(product);

            return product;
        }

        private List<Supplier> matchingSuppliers;
        private ISupplierRepository supplierRepository = new SupplierRepository();
        private IRepository<Product> productRepository = new Repository<Product>();
        private IRepository<Category> categoryRepository = new Repository<Category>();
    }
}
