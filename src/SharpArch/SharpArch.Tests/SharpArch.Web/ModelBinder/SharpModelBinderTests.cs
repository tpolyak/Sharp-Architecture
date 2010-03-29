using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using Moq;
using NUnit.Framework;
using SharpArch.Core.CommonValidator;
using SharpArch.Core.DomainModel;
using SharpArch.Core.NHibernateValidator.CommonValidatorAdapter;
using SharpArch.Core.PersistenceSupport;
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
                                         {"Employee.Reports", "3"}, 
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
            Assert.AreEqual(2,result.Reports.Count);
        }

        [Test]
        public void CanBindModelWithEntityCollection()
        {
            int id = 2;
            string employeeName = "Michael";

            // Arrange
            var formCollection = new NameValueCollection
                                     {
                                         {"Employee.Id", id.ToString()},
                                         {"Employee.Name", employeeName},
                                         {"Employee.Reports[0].Name", "Michael"},
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

        [Test]
        public void CanBindModelWithNestedEntities()
        {
            int id = 2;
            string employeeName = "Michael";
            string managerName = "Tobias";
            string managerManagerName = "Scott";

            // Arrange
            var formCollection = new NameValueCollection
                                     {
                                         {"Employee.Id", id.ToString()},
                                         {"Employee.Name", employeeName},
                                         {"Employee.Manager.Name", managerName},
                                         {"Employee.Manager.Manager.Name", managerManagerName}
                                     };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof (Employee));

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
            Assert.AreEqual(managerName, result.Manager.Name);
            Assert.AreEqual(managerManagerName, result.Manager.Manager.Name);
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            var mockRepository = new Mock<IRepositoryWithTypedId<Employee, int>>();
            var windsorContainer = new WindsorContainer();
            mockRepository.Setup(r => r.Get(It.IsAny<int>())).Returns((int newId) =>new Employee(newId));

            windsorContainer.Register(Component.For<IRepositoryWithTypedId<Employee, int>>().Instance(mockRepository.Object));
            windsorContainer.AddComponent("validator", typeof(IValidator), typeof(Validator));


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
