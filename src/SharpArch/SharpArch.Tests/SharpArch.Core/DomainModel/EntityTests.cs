using NUnit.Framework;
using SharpArch.Core.DomainModel;
using NUnit.Framework.SyntaxHelpers;
using SharpArch.Testing;
using System.Collections.Generic;

namespace Tests.SharpArch.Core.DomainModel
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void CanHaveEntityWithoutDomainSignatureProperties() {
            ObjectWithNoDomainSignatureProperties invalidEntity = 
                new ObjectWithNoDomainSignatureProperties();

            invalidEntity.GetSignatureProperties();
        }

        [Test]
        public void Transient_entity_without_domain_signature_should_return_consistent_hashcode()
        {
            var sut = new ObjectWithNoDomainSignatureProperties();

            Assert.That(sut.GetHashCode(), Is.EqualTo(sut.GetHashCode()));
        }

        [Test]
        public void Two_transient_entities_without_signature_properties_generate_different_hashcodes()
        {
            var sut1 = new ObjectWithNoDomainSignatureProperties();
            var sut2 = new ObjectWithNoDomainSignatureProperties();
            
            Assert.That(sut1.GetHashCode(), Is.Not.EqualTo(sut2.GetHashCode()));
        }

        [Test]
        public void Entity_with_no_signature_properties_preserves_hashcode_when_transitioning_from_transient_to_persistent()
        {
            var sut = new ObjectWithNoDomainSignatureProperties();

            Assert.That(sut.IsTransient());

            var hashcodeWhenTransient = sut.GetHashCode();

            sut.SetIdTo(1);

            Assert.That(sut.IsTransient(), Is.False);
            Assert.That(sut.GetHashCode(), Is.EqualTo(hashcodeWhenTransient));
        }

        [Test]
        public void Two_persistent_entities_with_no_signature_properties_and_different_ids_generate_different_hashcodes()
        {
            var sut1 = new ObjectWithNoDomainSignatureProperties().SetIdTo(1);
            var sut2 = new ObjectWithNoDomainSignatureProperties().SetIdTo(2);

            Assert.That(sut1.GetHashCode(), Is.Not.EqualTo(sut2.GetHashCode()));
        }

        [Test]
        public void Two_persistent_entities_with_no_signature_properties_and_equal_ids_generate_equal_hashcodes()
        {
            var sut1 = new ObjectWithNoDomainSignatureProperties().SetIdTo(1);
            var sut2 = new ObjectWithNoDomainSignatureProperties().SetIdTo(1);

            Assert.That(sut1.GetHashCode(), Is.EqualTo(sut2.GetHashCode()));
        }

        [Test]
        public void Transient_entity_with_domain_signature_should_return_consistent_hashcode()
        {
            var sut = new ObjectWithOneDomainSignatureProperty { Age = 1 };

            Assert.That(sut.GetHashCode(), Is.EqualTo(sut.GetHashCode()));
        }

        [Test]
        public void Two_transient_entities_with_different_values_of_domain_signature_generate_different_hashcodes()
        {
            var sut1 = new ObjectWithOneDomainSignatureProperty {Age = 1};
            var sut2 = new ObjectWithOneDomainSignatureProperty {Age = 2};

            Assert.That(sut1.GetHashCode(), Is.Not.EqualTo(sut2.GetHashCode()));
        }

        [Test]
        public void Two_transient_entitites_with_equal_values_of_domain_signature_generate_equal_hashcodes()
        {
            var sut1 = new ObjectWithOneDomainSignatureProperty { Age = 1 };
            var sut2 = new ObjectWithOneDomainSignatureProperty { Age = 1 };

            Assert.That(sut1.GetHashCode(), Is.EqualTo(sut2.GetHashCode()));
        }

        [Test]
        public void Transient_entity_with_domain_signature_preserves_hashcode_temporarily_when_its_domain_signature_changes()
        {
            var sut = new ObjectWithOneDomainSignatureProperty { Age = 1 };

            var initialHashcode = sut.GetHashCode();

            sut.Age = 2;

            Assert.That(sut.GetHashCode(), Is.EqualTo(initialHashcode));
        }

        [Test]
        public void Entity_with_domain_signature_preserves_hashcode_when_transitioning_from_transient_to_persistent()
        {
            var sut = new ObjectWithOneDomainSignatureProperty {Age = 1};

            Assert.That(sut.IsTransient());

            var hashcodeWhenTransient = sut.GetHashCode();

            sut.SetIdTo(1);

            Assert.That(sut.IsTransient(), Is.False);
            Assert.That(sut.GetHashCode(), Is.EqualTo(hashcodeWhenTransient));
        }

        [Test]
        public void Two_persistent_entities_with_equal_domain_signature_and_different_ids_generate_different_hashcodes()
        {
            var sut1 = new ObjectWithOneDomainSignatureProperty {Age = 1}.SetIdTo(1);
            var sut2 = new ObjectWithOneDomainSignatureProperty {Age = 1}.SetIdTo(2);

            Assert.That(sut1.GetHashCode(), Is.Not.EqualTo(sut2.GetHashCode()));
        }

        [Test]
        public void Two_persistent_entities_with_different_domain_signature_and_equal_ids_generate_equal_hashcodes()
        {
            var sut1 = new ObjectWithOneDomainSignatureProperty {Age = 1}.SetIdTo(1);
            var sut2 = new ObjectWithOneDomainSignatureProperty {Age = 2}.SetIdTo(1);

            Assert.That(sut1.GetHashCode(), Is.EqualTo(sut2.GetHashCode()));
        }

        [Test]
        public void KeepsConsistentHashThroughLifetimeOfTransientObject() {
            ObjectWithOneDomainSignatureProperty object1 = new ObjectWithOneDomainSignatureProperty();
            int initialHash = object1.GetHashCode();

            object1.Age = 13;
            object1.Name = "Foo";

            Assert.AreEqual(initialHash, object1.GetHashCode());

            object1.Age = 14;
            Assert.AreEqual(initialHash, object1.GetHashCode());
        }

        [Test]
        public void KeepsConsistentHashThroughLifetimeOfPersistentObject() {
            ObjectWithOneDomainSignatureProperty object1 = new ObjectWithOneDomainSignatureProperty();
            EntityIdSetter.SetIdOf<int>(object1, 1);
            int initialHash = object1.GetHashCode();

            object1.Age = 13;
            object1.Name = "Foo";

            Assert.AreEqual(initialHash, object1.GetHashCode());

            object1.Age = 14;
            Assert.AreEqual(initialHash, object1.GetHashCode());
        }

        [Test]
        public void CanCompareDomainObjectsWithOnlySomePropertiesBeingPartOfDomainSignature() {
            ObjectWithOneDomainSignatureProperty object1 = new ObjectWithOneDomainSignatureProperty();
            ObjectWithOneDomainSignatureProperty object2 = new ObjectWithOneDomainSignatureProperty();
            Assert.That(object1, Is.EqualTo(object2));

            object1.Age = 13;
            object2.Age = 13;
            // Name property isn't included in comparison
            object1.Name = "Foo";
            object2.Name = "Bar";
            Assert.That(object1, Is.EqualTo(object2));

            object1.Age = 14;
            Assert.That(object1, Is.Not.EqualTo(object2));
        }

        [Test]
        public void CanCompareDomainObjectsWithAllPropertiesBeingPartOfDomainSignature() {
            ObjectWithAllDomainSignatureProperty object1 = new ObjectWithAllDomainSignatureProperty();
            ObjectWithAllDomainSignatureProperty object2 = new ObjectWithAllDomainSignatureProperty();
            Assert.That(object1, Is.EqualTo(object2));

            object1.Age = 13;
            object2.Age = 13;
            object1.Name = "Foo";
            object2.Name = "Foo";
            Assert.That(object1, Is.EqualTo(object2));

            object1.Name = "Bar";
            Assert.That(object1, Is.Not.EqualTo(object2));

            object1.Name = null;
            Assert.That(object1, Is.Not.EqualTo(object2));

            object2.Name = null;
            Assert.That(object1, Is.EqualTo(object2));
        }

        [Test]
        public void CanCompareInheritedDomainObjects() {
            InheritedObjectWithExtraDomainSignatureProperty object1 =
                new InheritedObjectWithExtraDomainSignatureProperty();
            InheritedObjectWithExtraDomainSignatureProperty object2 =
                new InheritedObjectWithExtraDomainSignatureProperty();
            Assert.That(object1, Is.EqualTo(object2));

            object1.Age = 13;
            object1.IsLiving = true;
            object2.Age = 13;
            object2.IsLiving = true;
            // Address property isn't included in comparison
            object1.Address = "123 Oak Ln.";
            object2.Address = "Nightmare on Elm St.";
            Assert.That(object1, Is.EqualTo(object2));

            object1.IsLiving = false;
            Assert.That(object1, Is.Not.EqualTo(object2));
        }

        [Test]
        public void WontGetConfusedWithOutsideCases() {
            ObjectWithIdenticalTypedProperties object1 =
                new ObjectWithIdenticalTypedProperties();
            ObjectWithIdenticalTypedProperties object2 =
                new ObjectWithIdenticalTypedProperties();

            object1.Address = "Henry";
            object1.Name = "123 Lane St.";
            object2.Address = "123 Lane St.";
            object2.Name = "Henry";
            Assert.That(object1, Is.Not.EqualTo(object2));

            object1.Address = "Henry";
            object1.Name = null;
            object2.Address = "Henri";
            object2.Name = null;
            Assert.That(object1, Is.Not.EqualTo(object2));

            object1.Address = null;
            object1.Name = "Supercalifragilisticexpialidocious";
            object2.Address = null;
            object2.Name = "Supercalifragilisticexpialidocious";
            Assert.That(object1, Is.EqualTo(object2));

            object1.Name = "Supercalifragilisticexpialidocious";
            object2.Name = "Supercalifragilisticexpialidociouz";
            Assert.That(object1, Is.Not.EqualTo(object2));
        }

        [Test]
        public void CanCompareObjectsWithComplexProperties() {
            ObjectWithComplexProperties object1 = new ObjectWithComplexProperties();
            ObjectWithComplexProperties object2 = new ObjectWithComplexProperties();

            Assert.AreEqual(object1, object2);

            object1.Address = new AddressBeingDomainSignatureComparble() {
                Address1 = "123 Smith Ln.",
                Address2 = "Suite 201",
                ZipCode = 12345
            };
            Assert.AreNotEqual(object1, object2);

            // Set the address of the 2nd to be different to the address of the first
            object2.Address = new AddressBeingDomainSignatureComparble() {
                Address1 = "123 Smith Ln.",
                // Address2 isn't marked as being part of the domain signature; 
                // therefore, it WON'T be used in the equality comparison
                Address2 = "Suite 402",
                ZipCode = 98765
            };
            Assert.AreNotEqual(object1, object2);

            // Set the address of the 2nd to be the same as the first
            object2.Address.ZipCode = 12345;
            Assert.AreEqual(object1, object2);

            object1.Phone = new PhoneBeingNotDomainObject() {
                PhoneNumber = "(555) 555-5555"
            };
            Assert.AreNotEqual(object1, object2);

            // IMPORTANT: Note that even though the phone number below has the same value as the 
            // phone number on object1, they're not domain signature comparable; therefore, the
            // "out of the box" equality will be used which shows them as being different objects.
            object2.Phone = new PhoneBeingNotDomainObject() {
                PhoneNumber = "(555) 555-5555"
            };
            Assert.AreNotEqual(object1, object2);

            // Observe as we replace the object1.Phone with an object that isn't domain-signature
            // comparable, but DOES have an overridden Equals which will return true if the phone
            // number properties are equal.
            object1.Phone = new PhoneBeingNotDomainObjectButWithOverriddenEquals() {
                PhoneNumber = "(555) 555-5555"
            };
            Assert.AreEqual(object1, object2);
        }

        /// <summary>
        /// This is a nonsense object; i.e., it doesn't make sense to have 
        /// an entity without a domain signature.
        /// </summary>
        private class ObjectWithNoDomainSignatureProperties : Entity
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class ObjectWithOneDomainSignatureProperty : Entity
        {
            public string Name { get; set; }

            [DomainSignature]
            public int Age { get; set; }
        }

        private class ObjectWithAllDomainSignatureProperty : Entity
        {
            [DomainSignature]
            public string Name { get; set; }

            [DomainSignature]
            public int Age { get; set; }
        }

        private class InheritedObjectWithExtraDomainSignatureProperty : ObjectWithOneDomainSignatureProperty
        {
            public string Address { get; set; }

            [DomainSignature]
            public bool IsLiving { get; set; }
        }

        private class ObjectWithIdenticalTypedProperties : Entity
        {
            [DomainSignature]
            public string Name { get; set; }

            [DomainSignature]
            public string Address { get; set; }
        }

        #region ObjectWithComplexProperties

        private class ObjectWithComplexProperties : Entity
        {
            [DomainSignature]
            public string Name { get; set; }

            [DomainSignature]
            public AddressBeingDomainSignatureComparble Address { get; set; }

            [DomainSignature]
            public PhoneBeingNotDomainObject Phone { get; set; }
        }

        private class AddressBeingDomainSignatureComparble : Entity
        {
            [DomainSignature]
            public string Address1 { get; set; }

            public string Address2 { get; set; }

            [DomainSignature]
            public int ZipCode { get; set; }
        }

        private class PhoneBeingNotDomainObject
        {
            public string PhoneNumber { get; set; }
            public string Extension { get; set; }
        }

        private class PhoneBeingNotDomainObjectButWithOverriddenEquals : PhoneBeingNotDomainObject
        {
            public override bool Equals(object obj) {
                PhoneBeingNotDomainObject compareTo =
                    obj as PhoneBeingNotDomainObject;

                return (compareTo != null &&
                    PhoneNumber.Equals(compareTo.PhoneNumber));
            }

            public override int GetHashCode() {
                return base.GetHashCode();
            }
        }

        #endregion

        #region Carry-Over tests from when Entity was split from an object called PersistentObject

        [Test]
        public void CanCompareEntitys() {
            ObjectWithIntId object1 = new ObjectWithIntId() { Name = "Acme" };
            ObjectWithIntId object2 = new ObjectWithIntId() { Name = "Anvil" };

            Assert.That(object1, Is.Not.EqualTo(null));
            Assert.That(object1, Is.Not.EqualTo(object2));

            EntityIdSetter.SetIdOf(object1, 10);
            EntityIdSetter.SetIdOf(object2, 10);

            // Even though the "business value signatures" are different, the persistent Ids 
            // were the same.  Call me crazy, but I put that much trust into persisted Ids.
            Assert.That(object1, Is.EqualTo(object2));
            Assert.That(object1.GetHashCode(), Is.EqualTo(object2.GetHashCode()));

            ObjectWithIntId object3 = new ObjectWithIntId() { Name = "Acme" };

            // Since object1 has an Id but object3 doesn't, they won't be equal
            // even though their signatures are the same
            Assert.That(object1, Is.Not.EqualTo(object3));

            ObjectWithIntId object4 = new ObjectWithIntId() { Name = "Acme" };

            // object3 and object4 are both transient and share the same signature
            Assert.That(object3, Is.EqualTo(object4));
        }

        [Test]
        public void CanCompareEntitiesWithAssignedIds() {
            ObjectWithAssignedId object1 = new ObjectWithAssignedId() { Name = "Acme" };
            ObjectWithAssignedId object2 = new ObjectWithAssignedId() { Name = "Anvil" };

            Assert.That(object1, Is.Not.EqualTo(null));
            Assert.That(object1, Is.Not.EqualTo(object2));

            object1.SetAssignedIdTo("AAAAA");
            object2.SetAssignedIdTo("AAAAA");

            // Even though the "business value signatures" are different, the persistent Ids 
            // were the same.  Call me crazy, but I put that much trust into persisted Ids.
            Assert.That(object1, Is.EqualTo(object2));

            ObjectWithAssignedId object3 = new ObjectWithAssignedId() { Name = "Acme" };

            // Since object1 has an Id but object3 doesn't, they won't be equal
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

            EntityIdSetter.SetIdOf(object1Type, 1);
            EntityIdSetter.SetIdOf(object2Type, 1);

            Assert.That(object1Type, Is.Not.EqualTo(object2Type));
        }

        private class ObjectWithIntId : Entity
        {
            [DomainSignature]
            public string Name { get; set; }
        }

        private class ObjectWithAssignedId : EntityWithTypedId<string>, IHasAssignedId<string>
        {
            [DomainSignature]
            public string Name { get; set; }

            public void SetAssignedIdTo(string assignedId) {
                Id = assignedId;
            }
        }

        private class Object1 : Entity { }
        private class Object2 : Entity { }

        #region Comprehensive unit tests provided by Brian Nicoloff

        [SetUp]
        public void Setup() {
            _obj = new MockEntityObjectWithDefaultId {
                FirstName = "FName1",
                LastName = "LName1",
                Email = "testus...@mail.com"
            };
            _sameObj = new MockEntityObjectWithDefaultId {
                FirstName = "FName1",
                LastName = "LName1",
                Email = "testus...@mail.com"
            };
            _diffObj = new MockEntityObjectWithDefaultId {
                FirstName = "FName2",
                LastName = "LName2",
                Email = "testuse...@mail.com"
            };
            _objWithId = new MockEntityObjectWithSetId {
                FirstName = "FName1",
                LastName = "LName1",
                Email = "testus...@mail.com"
            };
            _sameObjWithId = new MockEntityObjectWithSetId {
                FirstName = "FName1",
                LastName = "LName1",
                Email = "testus...@mail.com"
            };
            _diffObjWithId = new MockEntityObjectWithSetId {
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
        public void DoesDefaultEntityEqualsOverrideWorkWhenNoIdIsAssigned() {
            Assert.That(_obj.Equals(_sameObj));
            Assert.That(!_obj.Equals(_diffObj));
            Assert.That(!_obj.Equals(new MockEntityObjectWithDefaultId()));
        }

        [Test]
        public void DoEqualDefaultEntitiesWithNoIdsGenerateSameHashCodes() {
            Assert.That(_obj.GetHashCode().Equals(_sameObj.GetHashCode()));
        }

        [Test]
        public void DoEqualDefaultEntitiesWithMatchingIdsGenerateDifferentHashCodes() {
            Assert.That(!_obj.GetHashCode().Equals(_diffObj.GetHashCode()));
        }

        [Test]
        public void DoDefaultEntityEqualsOverrideWorkWhenIdIsAssigned() {
            _obj.SetAssignedIdTo(1);
            _diffObj.SetAssignedIdTo(1);
            Assert.That(_obj.Equals(_diffObj));
        }

        [Test]
        public void DoesEntityEqualsOverrideWorkWhenNoIdIsAssigned() {
            Assert.That(_objWithId.Equals(_sameObjWithId));
            Assert.That(!_objWithId.Equals(_diffObjWithId));
            Assert.That(!_objWithId.Equals(new MockEntityObjectWithSetId()));
        }

        [Test]
        public void DoEqualEntitiesWithNoIdsGenerateSameHashCodes() {
            Assert.That(_objWithId.GetHashCode().Equals(_sameObjWithId.GetHashCode()));
        }

        [Test]
        public void DoEqualEntitiesWithMatchingIdsGenerateDifferentHashCodes() {
            Assert.That(!_objWithId.GetHashCode().Equals(_diffObjWithId.GetHashCode()));
        }

        [Test]
        public void DoEntityEqualsOverrideWorkWhenIdIsAssigned() {
            _objWithId.SetAssignedIdTo("1");
            _diffObjWithId.SetAssignedIdTo("1");
            Assert.That(_objWithId.Equals(_diffObjWithId));
        }

        private class MockEntityObjectWithSetId :
            MockEntityObjectBase<string>, IHasAssignedId<string>
        {
            public void SetAssignedIdTo(string assignedId) {
                Id = assignedId;
            }
        }

        private class MockEntityObjectWithDefaultId :
            MockEntityObjectBase, IHasAssignedId<int>
        {
            public void SetAssignedIdTo(int assignedId) {
                Id = assignedId;
            }
        }

        private class MockEntityObjectBase :
            MockEntityObjectBase<int> { }

        public class MockEntityObjectBase<T> :
            EntityWithTypedId<T>
        {
            [DomainSignature]
            public string FirstName { get; set; }

            [DomainSignature]
            public string LastName { get; set; }

            public string Email { get; set; }
        }

        private MockEntityObjectWithDefaultId _obj;
        private MockEntityObjectWithDefaultId _sameObj;
        private MockEntityObjectWithDefaultId _diffObj;
        private MockEntityObjectWithSetId _objWithId;
        private MockEntityObjectWithSetId _sameObjWithId;
        private MockEntityObjectWithSetId _diffObjWithId;

        #endregion

        #endregion
    }
}
