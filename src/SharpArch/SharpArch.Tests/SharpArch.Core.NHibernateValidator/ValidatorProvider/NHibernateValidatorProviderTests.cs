using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            [NotNull(Message = "not_null_message")]
            public string NotNullProperty { get; set; }

            [NotEmpty(Message = "not_empty_message")]
            public string NotEmptyStringProperty { get; set; }

            [LengthAttribute(Message = "length_message", Min = 3, Max = 10)]
            public string LengthProperty { get; set; }

            [Min(Message = "min_message", Value = 3)]
            public string MinProperty { get; set; }

            [Max(Message = "max_message", Value = 10)]
            public string MaxProperty { get; set; }

            [NHibernate.Validator.Constraints.Range(Message = "range_message", Min = 3, Max = 10)]
            public int RangeProperty { get; set; }

            [Pattern(Message = "pattern_message", Regex = "[a-zA-Z]{3,10}")]
            public string PatternProperty { get; set; }
        }

        [SetUp]
        public void SetUp()
        {
            _validatorProvider = new NHibernateValidatorProvider();
            _viewData = new ViewDataDictionary<TestModel>();
            _controllerContext = new ControllerContext();
        }
        
        [Test]
        public void ClientValidation_NotNullProperty()
        {
            ClientValidation_AssertRule(x => x.NotNullProperty, "required", "not_null_message");
        }
        
        [Test]
        public void ClientValidation_NotEmptyStringProperty()
        {
            ClientValidation_AssertRule(x => x.NotEmptyStringProperty, "required", "not_empty_message");
        }


        [Test]
        public void ClientValidation_LengthProperty()
        {
            var validationRule = ClientValidation_AssertRule(x => x.LengthProperty, "length", "length_message");
            Assert.That(validationRule.ValidationParameters["min"], Is.EqualTo(3));
            Assert.That(validationRule.ValidationParameters["max"], Is.EqualTo(10));
        }

        [Test]
        public void ClientValidation_MinPropertyProperty()
        {
            var validationRule = ClientValidation_AssertRule(x => x.MinProperty, "range", "min_message");
            Assert.That(validationRule.ValidationParameters["min"], Is.EqualTo(3));
        }

        [Test]
        public void ClientValidation_MaxPropertyProperty()
        {
            var validationRule = ClientValidation_AssertRule(x => x.MaxProperty, "range", "max_message");
            Assert.That(validationRule.ValidationParameters["max"], Is.EqualTo(10));
        }

        [Test]
        public void ClientValidation_PatternProperty()
        {
            var validationRule = ClientValidation_AssertRule(x => x.PatternProperty, "regex", "pattern_message");
            Assert.That(validationRule.ValidationParameters["pattern"], Is.EqualTo("[a-zA-Z]{3,10}"));
        }

        [Test]
        public void ClientValidation_RangeProperty()
        {
            var validationRule = ClientValidation_AssertRule(x => x.RangeProperty, "range", "range_message");
            Assert.That(validationRule.ValidationParameters["min"], Is.EqualTo(3));
            Assert.That(validationRule.ValidationParameters["max"], Is.EqualTo(10));
        }

        private ModelClientValidationRule ClientValidation_AssertRule<TValue>(Expression<Func<TestModel, TValue>> property, string validationType, string errorMessate)
        {
            var modelMetadata =
                ModelMetadata.FromLambdaExpression(property, _viewData);

            var modelValidators = _validatorProvider.GetValidators(modelMetadata, _controllerContext);

            Assert.That(modelValidators, Is.Not.Empty);

            var validationRules = modelValidators.SelectMany(x => x.GetClientValidationRules()).ToList();

            Assert.That(validationRules.Count, Is.EqualTo(1));

            var validationRule = validationRules[0];

            Assert.That(validationRule.ValidationType, Is.EqualTo(validationType));
            Assert.That(validationRule.ErrorMessage, Is.EqualTo(errorMessate));

            return validationRule;

        }
    }
}
