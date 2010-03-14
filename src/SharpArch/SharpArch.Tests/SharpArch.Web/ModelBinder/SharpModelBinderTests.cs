using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using NUnit.Framework;
using SharpArch.Core.DomainModel;
using SharpArch.Web.ModelBinder;

namespace Tests.SharpArch.Web.ModelBinder
{
    [TestFixture]
    class SharpModelBinderTests
    {
        [Test]
        public void CanBindSimpleModel()
        {
            int id = 2;
            string employeeName = "Michael";

            // Arrange
            var formCollection = new NameValueCollection
                                     {
                                         {"Employee.Id", id.ToString()},
                                         {"Employee.Name", employeeName},
                                         //{"Employee.Reports", "3"}, // these don't work without a repository configured.
                                         //{"Employee.Reports", "4"},
                                         {"Employee.Manager", "12"}
                                     };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(Employee));

            var bindingContext = new ModelBindingContext
            {
                ModelName = "Employee",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            DefaultModelBinder target = new SharpModelBinder();

            ControllerContext controllerContext = new ControllerContext();

            // Act
            Employee result = (Employee)target.BindModel(controllerContext, bindingContext);

            // Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(employeeName, result.Name);
        }

        #region TestClass

        private class Employee : Entity
        {
            public Employee()
            {
                Reports = new List<Employee>();
            }
            public string Name
            {
                get;
                set;
            }

            public Employee Manager
            {
                get; set;
            }

            public IList<Employee> Reports
            {
                get; protected set;
            }
        }

        #endregion
    }
}
