using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NHibernate.Validator.Constraints;
using NUnit.Framework;
using SharpArch.Core.NHibernateValidator.ValidatorProvider;

namespace Tests.SharpArch.Core.NHibernateValidator.ValidatorProvider
{
    [TestFixture]
    public class NHibernateValidatorProviderTests
    {
        private NHibernateValidatorProvider _validatorProvider;
        private ViewDataDictionary<TestModel> _viewData;
        private ControllerContext _controllerContext;

        class TestModel
        {
            [NotNull]
            public string NotNullProperty { get; set; }

            [NotEmpty]
            public string NotEmptyStringProperty { get; set; }
        }

        [SetUp]
        public void SetUp()
        {
            _validatorProvider = new NHibernateValidatorProvider();
            _viewData = new ViewDataDictionary<TestModel>();
            _controllerContext = new ControllerContext();
        }
        
        [Test]
        public void NotNullProperty()
        {
            var modelMetadata =
                ModelMetadata.FromLambdaExpression<TestModel, string>(x => x.NotNullProperty, _viewData);

            var modelValidators = _validatorProvider.GetValidators(modelMetadata, _controllerContext);

            Assert.That(modelValidators, Is.Not.Empty);

            var validationRules = modelValidators.SelectMany(x => x.GetClientValidationRules()).ToList();

            Assert.That(validationRules.Count, Is.EqualTo(1));

            var validationRule = validationRules[0];

            Assert.That(validationRule.ValidationType, Is.EqualTo("required"));

            //Assert.That(validationRule.ErrorMessage, Is.EqualTo(""));
        }
    }
}
