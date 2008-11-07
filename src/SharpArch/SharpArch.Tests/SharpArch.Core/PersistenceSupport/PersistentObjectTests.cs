using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core;
using NUnit.Framework.SyntaxHelpers;

namespace Tests.SharpArch.Core.PersistenceSupport
{
    [TestFixture]
    public class PersistentObjectTests
    {
        [Test]
        public void CanComparePersistentObjects() {
            ObjectWithIntId object1 = new ObjectWithIntId() { Name = "Acme" };
            ObjectWithIntId object2 = new ObjectWithIntId() { Name = "Anvil" };

            Assert.That(object1, Is.Not.EqualTo(null));
            Assert.That(object1, Is.Not.EqualTo(object2));

            PersistentObjectIdSetter<int>.SetIdOf(object1, 10);
            PersistentObjectIdSetter<int>.SetIdOf(object2, 10);

            // Even though the "business value signatures" are different, the persistent IDs 
            // were the same.  Call me crazy, but I put that much trust into persisted IDs.
            Assert.That(object1, Is.EqualTo(object2));

            ObjectWithIntId object3 = new ObjectWithIntId() { Name = "Acme" };

            // Since object1 has an ID but object3 doesn't, they won't be equal
            // even though their signatures are the same
            Assert.That(object1, Is.Not.EqualTo(object3));

            ObjectWithIntId object4 = new ObjectWithIntId() { Name = "Acme" };

            // object3 and object4 are both transient and share the same signature
            Assert.That(object3, Is.EqualTo(object4));
        }

        [Test]
        public void CanComparePersistentObjectsWithAssignedIds() {
            ObjectWithAssignedId object1 = new ObjectWithAssignedId() { Name = "Acme" };
            ObjectWithAssignedId object2 = new ObjectWithAssignedId() { Name = "Anvil" };

            Assert.That(object1, Is.Not.EqualTo(null));
            Assert.That(object1, Is.Not.EqualTo(object2));

            object1.SetAssignedIdTo("AAAAA");
            object2.SetAssignedIdTo("AAAAA");

            // Even though the "business value signatures" are different, the persistent IDs 
            // were the same.  Call me crazy, but I put that much trust into persisted IDs.
            Assert.That(object1, Is.EqualTo(object2));

            ObjectWithAssignedId object3 = new ObjectWithAssignedId() { Name = "Acme" };

            // Since object1 has an ID but object3 doesn't, they won't be equal
            // even though their signatures are the same
            Assert.That(object1, Is.Not.EqualTo(object3));

            ObjectWithAssignedId object4 = new ObjectWithAssignedId() { Name = "Acme" };

            // object3 and object4 are both transient and share the same signature
            Assert.That(object3, Is.EqualTo(object4));
        }

        [Test]
        public void CannotEquateObjectsWithSameIdButDifferentTypes() {
            Object1 object1Type = new Object1();
            Object2 object2Type = new Object2();

            PersistentObjectIdSetter<int>.SetIdOf(object1Type, 1);
            PersistentObjectIdSetter<int>.SetIdOf(object2Type, 1);

            Assert.That(object1Type, Is.Not.EqualTo(object2Type));
        }

        private class ObjectWithIntId : PersistentObject {
            [DomainSignature]
            public string Name { get; set; }
        }

        private class ObjectWithAssignedId : PersistentObjectWithTypedId<string>, IHasAssignedId<string>
        {
            [DomainSignature]
            public string Name { get; set; }

            public void SetAssignedIdTo(string assignedId) {
                ID = assignedId;
            }
        }

        private class Object1 : PersistentObject {}
        private class Object2 : PersistentObject { }

        #region Comprehensive unit tests provided by Brian Nicoloff

        [SetUp]
        public void Setup() {
            _obj = new MockPersistentDomainObjectWithDefaultId {
                FirstName = "FName1",
                LastName = "LName1",
                Email = "testus...@mail.com"
            };
            _sameObj = new MockPersistentDomainObjectWithDefaultId {
                FirstName = "FName1",
                LastName = "LName1",
                Email = "testus...@mail.com"
            };
            _diffObj = new MockPersistentDomainObjectWithDefaultId {
                FirstName = "FName2",
                LastName = "LName2",
                Email = "testuse...@mail.com"
            };
            _objWithId = new MockPersistentDomainObjectWithSetId {
                FirstName = "FName1",
                LastName = "LName1",
                Email = "testus...@mail.com"
            };
            _sameObjWithId = new MockPersistentDomainObjectWithSetId {
                FirstName = "FName1",
                LastName = "LName1",
                Email = "testus...@mail.com"
            };
            _diffObjWithId = new MockPersistentDomainObjectWithSetId {
                FirstName = "FName2",
                LastName = "LName2",
                Email = "testuse...@mail.com"
            };
        }

        [TearDown]
        public void Teardown() {
            _obj = null;
            _sameObj = null;
            _diffObj = null;
        }

        [Test]
        public void DoesDefaultPersistentObjectEqualsOverrideWorkWhenNoIdIsAssigned() {
            Assert.That(_obj.Equals(_sameObj));
            Assert.That(!_obj.Equals(_diffObj));
            Assert.That(!_obj.Equals(new MockPersistentDomainObjectWithDefaultId()));
        }

        [Test]
        public void DoEqualDefaultPersistentObjectsWithNoIdsGenerateSameHashCodes() {
            Assert.That(_obj.GetHashCode().Equals(_sameObj.GetHashCode()));
        }

        [Test]
        public void DoEqualDefaultPersistentObjectsWithMatchingIdsGenerateDifferentHashCodes() {
            Assert.That(!_obj.GetHashCode().Equals(_diffObj.GetHashCode()));
        }

        [Test]
        public void DoDefaultPersistentObjectEqualsOverrideWorkWhenIdIsAssigned() {
            _obj.SetAssignedIdTo(1);
            _diffObj.SetAssignedIdTo(1);
            Assert.That(_obj.Equals(_diffObj));
        }

        [Test]
        public void DoesPersistentObjectEqualsOverrideWorkWhenNoIdIsAssigned() {
            Assert.That(_objWithId.Equals(_sameObjWithId));
            Assert.That(!_objWithId.Equals(_diffObjWithId));
            Assert.That(!_objWithId.Equals(new MockPersistentDomainObjectWithSetId()));
        }

        [Test]
        public void DoEqualPersistentObjectsWithNoIdsGenerateSameHashCodes() {
            Assert.That(_objWithId.GetHashCode().Equals(_sameObjWithId.GetHashCode()));
        }

        [Test]
        public void DoEqualPersistentObjectsWithMatchingIdsGenerateDifferentHashCodes() {
            Assert.That(!_objWithId.GetHashCode().Equals(_diffObjWithId.GetHashCode()));
        }

        [Test]
        public void DoPersistentObjectEqualsOverrideWorkWhenIdIsAssigned() {
            _objWithId.SetAssignedIdTo("1");
            _diffObjWithId.SetAssignedIdTo("1");
            Assert.That(_objWithId.Equals(_diffObjWithId));
        }

        private class MockPersistentDomainObjectWithSetId :
            MockPersistentDomainObjectBase<string>, IHasAssignedId<string>
        {
            public void SetAssignedIdTo(string assignedId) {
                ID = assignedId;
            }
        }

        private class MockPersistentDomainObjectWithDefaultId :
            MockPersistentDomainObjectBase, IHasAssignedId<int>
        {
            public void SetAssignedIdTo(int assignedId) {
                ID = assignedId;
            }
        }

        private class MockPersistentDomainObjectBase :
            MockPersistentDomainObjectBase<int> { }

        public class MockPersistentDomainObjectBase<T> :
            PersistentObjectWithTypedId<T>
        {
            [DomainSignature]
            public string FirstName { get; set; }

            [DomainSignature]
            public string LastName { get; set; }

            public string Email { get; set; }
        }

        private MockPersistentDomainObjectWithDefaultId _obj;
        private MockPersistentDomainObjectWithDefaultId _sameObj;
        private MockPersistentDomainObjectWithDefaultId _diffObj;
        private MockPersistentDomainObjectWithSetId _objWithId;
        private MockPersistentDomainObjectWithSetId _sameObjWithId;
        private MockPersistentDomainObjectWithSetId _diffObjWithId;

        #endregion
    }
}
