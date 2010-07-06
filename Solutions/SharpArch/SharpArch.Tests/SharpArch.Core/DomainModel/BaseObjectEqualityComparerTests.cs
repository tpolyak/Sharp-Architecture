using NUnit.Framework;
using SharpArch.Core.DomainModel;
using System.Reflection;
using System.Collections.Generic;
using SharpArch.Testing;
using System.Linq;

namespace Tests.SharpArch.Core.DomainModel
{
    [TestFixture]
    public class BaseObjectEqualityComparerTests
    {
        [Test]
        public void CanCompareNulls() {
            BaseObjectEqualityComparer<BaseObject> comparer = new BaseObjectEqualityComparer<BaseObject>();
            Assert.That(comparer.Equals(null, null));
            Assert.That(comparer.Equals(null, new ConcreteBaseObject()), Is.False);
            Assert.That(comparer.Equals(new ConcreteBaseObject(), null), Is.False);
        }

        [Test]
        public void CannotSuccessfullyCompareDifferentlyTypedObjectsThatDeriveFromBaseObject() {
            BaseObjectEqualityComparer<BaseObject> comparer = new BaseObjectEqualityComparer<BaseObject>();

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
            BaseObjectEqualityComparer<BaseObject> comparer = new BaseObjectEqualityComparer<BaseObject>();

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
            BaseObjectEqualityComparer<BaseObject> comparer = new BaseObjectEqualityComparer<BaseObject>();

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
            BaseObjectEqualityComparer<BaseObject> comparer = new BaseObjectEqualityComparer<BaseObject>();

            ConcreteEntityWithNoDomainSignatureProperties object1 = new ConcreteEntityWithNoDomainSignatureProperties() {
                Name = "Whatever"
            };
            ConcreteEntityWithNoDomainSignatureProperties object2 = new ConcreteEntityWithNoDomainSignatureProperties() {
                Name = "asdf"
            };
            Assert.That(comparer.Equals(object1, object2), Is.False);

            EntityIdSetter.SetIdOf<int>(object1, 1);
            EntityIdSetter.SetIdOf<int>(object2, 1);
            Assert.That(comparer.Equals(object1, object2));
        }

        [Test]
        public void CanCompareEntitiesWithDomainSignatureProperties() {
            BaseObjectEqualityComparer<Entity> comparer = new BaseObjectEqualityComparer<Entity>();

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

        [Test]
        public void CanBeUsedByLinqSetOperatorsSuchAsIntersect() {
            IList<ConcreteEntityWithDomainSignatureProperties> objects1 = new List<ConcreteEntityWithDomainSignatureProperties>();
            ConcreteEntityWithDomainSignatureProperties object1 = new ConcreteEntityWithDomainSignatureProperties() {
                Name = "Billy McCafferty",
            };
            EntityIdSetter.SetIdOf<int>(object1, 2);
            objects1.Add(object1);

            IList<ConcreteEntityWithDomainSignatureProperties> objects2 = new List<ConcreteEntityWithDomainSignatureProperties>();
            ConcreteEntityWithDomainSignatureProperties object2 = new ConcreteEntityWithDomainSignatureProperties() {
                Name = "Jimi Hendrix",
            };
            EntityIdSetter.SetIdOf<int>(object2, 1);
            objects2.Add(object2);
            ConcreteEntityWithDomainSignatureProperties object3 = new ConcreteEntityWithDomainSignatureProperties() {
                Name = "Doesn't Matter since the Id will match and the presedence of the domain signature will go overridden",
            };
            EntityIdSetter.SetIdOf<int>(object3, 2);
            objects2.Add(object3);

            Assert.That(objects1.Intersect(objects2,
                new BaseObjectEqualityComparer<ConcreteEntityWithDomainSignatureProperties>()).Count(),
                Is.EqualTo(1));
            Assert.AreEqual(objects1.Intersect(objects2,
                new BaseObjectEqualityComparer<ConcreteEntityWithDomainSignatureProperties>()).First(), object1);
            Assert.AreEqual(objects1.Intersect(objects2,
                new BaseObjectEqualityComparer<ConcreteEntityWithDomainSignatureProperties>()).First(), object3);
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
