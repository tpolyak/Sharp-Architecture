namespace Tests.SharpArch.Core.NHibernateValidator.ValidatorProvider
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    using NHibernate.Validator.Constraints;

    using NUnit.Framework;

    using global::SharpArch.Core.NHibernateValidator.ValidatorProvider;

    [TestFixture]
    public class NHibernateValidatorProviderTests
    {
        private ControllerContext _controllerContext;

        private NHibernateValidatorProvider _validatorProvider;

        private ViewDataDictionary<TestModel> _viewData;

        [Test]
        public void ClientValidation_LengthProperty()
        {
            var validationRule = this.ClientValidation_AssertRule(
                x => x.LengthProperty, "stringLength", "length_message");
            Assert.That(validationRule.ValidationParameters["minimumLength"], Is.EqualTo(3));
            Assert.That(validationRule.ValidationParameters["maximumLength"], Is.EqualTo(10));
        }

        [Test]
        public void ClientValidation_MaxPropertyProperty()
        {
            var validationRule = this.ClientValidation_AssertRule(x => x.MaxProperty, "range", "max_message");
            Assert.That(validationRule.ValidationParameters["maximum"], Is.EqualTo(10));
        }

        [Test]
        public void ClientValidation_MinPropertyProperty()
        {
            var validationRule = this.ClientValidation_AssertRule(x => x.MinProperty, "range", "min_message");
            Assert.That(validationRule.ValidationParameters["minimum"], Is.EqualTo(3));
        }

        [Test]
        public void ClientValidation_NotEmptyStringProperty()
        {
            this.ClientValidation_AssertRule(x => x.NotEmptyStringProperty, "required", "not_empty_message");
        }

        [Test]
        public void ClientValidation_NotNullProperty()
        {
            this.ClientValidation_AssertRule(x => x.NotNullProperty, "required", "not_null_message");
        }

        [Test]
        public void ClientValidation_PatternProperty()
        {
            var validationRule = this.ClientValidation_AssertRule(
                x => x.PatternProperty, "regularExpression", "pattern_message");
            Assert.That(validationRule.ValidationParameters["pattern"], Is.EqualTo("[a-zA-Z]{3,10}"));
        }

        [Test]
        public void ClientValidation_RangeProperty()
        {
            var validationRule = this.ClientValidation_AssertRule(x => x.RangeProperty, "range", "range_message");
            Assert.That(validationRule.ValidationParameters["minimum"], Is.EqualTo(3));
            Assert.That(validationRule.ValidationParameters["maximum"], Is.EqualTo(10));
        }

        [SetUp]
        public void SetUp()
        {
            this._validatorProvider = new NHibernateValidatorProvider();
            this._viewData = new ViewDataDictionary<TestModel>();
            this._controllerContext = new ControllerContext();
        }

        private ModelClientValidationRule ClientValidation_AssertRule<TValue>(
            Expression<Func<TestModel, TValue>> property, string validationType, string errorMessate)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(property, this._viewData);

            var modelValidators = this._validatorProvider.GetValidators(modelMetadata, this._controllerContext);

            Assert.That(modelValidators, Is.Not.Empty);

            var validationRules = modelValidators.SelectMany(x => x.GetClientValidationRules()).ToList();

            Assert.That(validationRules.Count, Is.EqualTo(1));

            var validationRule = validationRules[0];

            Assert.That(validationRule.ValidationType, Is.EqualTo(validationType));
            Assert.That(validationRule.ErrorMessage, Is.EqualTo(errorMessate));

            return validationRule;
        }

        private class TestModel
        {
            [LengthAttribute(Message = "length_message", Min = 3, Max = 10)]
            public string LengthProperty { get; set; }

            [Max(Message = "max_message", Value = 10)]
            public string MaxProperty { get; set; }
            [Min(Message = "min_message", Value = 3)]
            public string MinProperty { get; set; }
            [NotEmpty(Message = "not_empty_message")]
            public string NotEmptyStringProperty { get; set; }
            [NotNull(Message = "not_null_message")]
            public string NotNullProperty { get; set; }

            [Pattern(Message = "pattern_message", Regex = "[a-zA-Z]{3,10}")]
            public string PatternProperty { get; set; }
            [NHibernate.Validator.Constraints.Range(Message = "range_message", Min = 3, Max = 10)]
            public int RangeProperty { get; set; }
        }
    }
}