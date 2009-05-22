using NUnit.Framework;
using SharpArch.Core.DomainModel;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework.SyntaxHelpers;
using SharpArch.Testing;

namespace Tests.SharpArch.Core.DomainModel
{
    [TestFixture]
    public class BaseObjectEqualityComparerTests
    {
        [Test]
        public void CanCompareNulls() {
            BaseObjectEqualityComparer comparer = new BaseObjectEqualityComparer();
            Assert.That(comparer.Equals(null, null));
            Assert.That(comparer.Equals(null, new ConcreteBaseObject()), Is.False);
            Assert.That(comparer.Equals(new ConcreteBaseObject(), null), Is.False);
        }

        [Test]
        public void CannotSuccessfullyCompareDifferentlyTypedObjectsThatDeriveFromBaseObject() {
            BaseObjectEqualityComparer comparer = new BaseObjectEqualityComparer();

            ConcreteBaseObject object1 = new ConcreteBaseObject() {
                Name = "Whatever"
            };
            ConcreteValueObject object2 = new ConcreteValueObject() {
                Name = "Whatever"
            };

            Assert.That(comparer.Equals(object1, object2), Is.False);
        }

        [Test]
        public void CanCompareBaseObjects() {
            BaseObjectEqualityComparer comparer = new BaseObjectEqualityComparer();

            ConcreteBaseObject object1 = new ConcreteBaseObject() {
                Name = "Whatever"
            };
            ConcreteBaseObject object2 = new ConcreteBaseObject() {
                Name = "Whatever"
            };
            Assert.That(comparer.Equals(object1, object2));

            object2.Name = "Mismatch";
            Assert.That(comparer.Equals(object1, object2), Is.False);
        }

        [Test]
        public void CanCompareValueObjects() {
            BaseObjectEqualityComparer comparer = new BaseObjectEqualityComparer();

            ConcreteValueObject object1 = new ConcreteValueObject() {
                Name = "Whatever"
            };
            ConcreteValueObject object2 = new ConcreteValueObject() {
                Name = "Whatever"
            };
            Assert.That(comparer.Equals(object1, object2));

            object2.Name = "Mismatch";
            Assert.That(comparer.Equals(object1, object2), Is.False);
        }

        [Test]
        public void CanCompareEntitiesWithNoDomainSignatureProperties() {
            BaseObjectEqualityComparer comparer = new BaseObjectEqualityComparer();

            ConcreteEntityWithNoDomainSignatureProperties object1 = new ConcreteEntityWithNoDomainSignatureProperties() {
                Name = "Whatever"
            };
            ConcreteEntityWithNoDomainSignatureProperties object2 = new ConcreteEntityWithNoDomainSignatureProperties() {
                Name = "Whatever"
            };
            Assert.That(comparer.Equals(object1, object2), Is.False);

            EntityIdSetter.SetIdOf<int>(object1, 1);
            EntityIdSetter.SetIdOf<int>(object2, 1);
            Assert.That(comparer.Equals(object1, object2));
        }

        [Test]
        public void CanCompareEntitiesWithDomainSignatureProperties() {
            BaseObjectEqualityComparer comparer = new BaseObjectEqualityComparer();

            ConcreteEntityWithDomainSignatureProperties object1 = new ConcreteEntityWithDomainSignatureProperties() {
                Name = "Whatever"
            };
            ConcreteEntityWithDomainSignatureProperties object2 = new ConcreteEntityWithDomainSignatureProperties() {
                Name = "Whatever"
            };
            Assert.That(comparer.Equals(object1, object2));

            object2.Name = "Mismatch";
            Assert.That(comparer.Equals(object1, object2), Is.False);

            EntityIdSetter.SetIdOf<int>(object1, 1);
            EntityIdSetter.SetIdOf<int>(object2, 1);
            Assert.That(comparer.Equals(object1, object2));
        }

        private class ConcreteBaseObject : BaseObject
        {
            protected override IEnumerable<PropertyInfo> GetTypeSpecificSignatureProperties() {
                return GetType().GetProperties();
            }

            public string Name { get; set; }
        }

        private class ConcreteEntityWithNoDomainSignatureProperties : Entity
        {
            public string Name { get; set; }
        }

        private class ConcreteEntityWithDomainSignatureProperties : Entity
        {
            [DomainSignature]
            public string Name { get; set; }
        }

        private class ConcreteValueObject : ValueObject
        {
            public string Name { get; set; }
        }
    }
}
