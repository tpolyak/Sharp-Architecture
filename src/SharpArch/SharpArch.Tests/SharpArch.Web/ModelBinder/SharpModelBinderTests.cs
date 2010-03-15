using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using Moq;
using NUnit.Framework;
using SharpArch.Core.DomainModel;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Data.NHibernate;
using SharpArch.Web.ModelBinder;

namespace Tests.SharpArch.Web.ModelBinder
{
    [TestFixture]
    public class SharpModelBinderTests
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

        [Test]
        public void CanBindModelWithCollection()
        {
            int id = 2;
            string employeeName = "Michael";
            string employee2Id = "3";
            string employee2Name = "Alec";

            // Arrange
            var formCollection = new NameValueCollection
                                     {
                                         {"Employee.Id", id.ToString()},
                                         {"Employee.Name", employeeName},
                                         {"Employee.Reports", "3"}, // these don't work without a repository configured.
                                         {"Employee.Reports", "4"},
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
            Assert.GreaterOrEqual(result.Reports.Count, 2);
        }

        [Test]
        public void CanBindModelWithEntityCollection()
        {
            int id = 2;
            string employeeName = "Michael";
            string employee2Id = "3";
            string employee2Name = "Alec";

            // Arrange
            var formCollection = new NameValueCollection
                                     {
                                         {"Employee.Id", id.ToString()},
                                         {"Employee.Name", employeeName},
                                         //{"Employee.Reports[0].Id", "3"}, // these don't work without a repository configured.
                                         //{"Employee.Reports[1].Id", "4"},
                                         {"Employee.Reports[0].Name", "Michael"}, // these don't work without a repository configured.
                                         {"Employee.Reports[1].Name", "Alec"},
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
            Assert.GreaterOrEqual(result.Reports.Count, 2);
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            var mockRepository = new Mock<IRepositoryWithTypedId<Employee, int>>();
            var windsorContainer = new WindsorContainer();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns((int newId) =>new Employee(newId));

            windsorContainer.Register(
                Component.For<IRepositoryWithTypedId<Employee, int>>().Instance(mockRepository.Object));


            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(windsorContainer));
        }

        #region TestClass

        public class Employee : Entity
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

            public Employee(int id)
            {
                Id = id;
            }
        }

        #endregion
    }
}
