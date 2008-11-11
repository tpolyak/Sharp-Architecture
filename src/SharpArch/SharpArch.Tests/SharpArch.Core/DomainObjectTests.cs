using NUnit.Framework;
using SharpArch.Core;
using NUnit.Framework.SyntaxHelpers;
using SharpArch.Core.PersistenceSupport;
using System.Diagnostics;

namespace Tests.SharpArch.Core
{
    [TestFixture]
    public class DomainObjectTests
    {
        [Test]
        public void CanComputeConsistentHashWithDomainSignatureProperties() {
            ObjectWithOneDomainSignatureProperty object1 = new ObjectWithOneDomainSignatureProperty();
            int defaultHash = object1.GetHashCode();

            object1.Age = 13;
            int domainSignatureEffectedHash = object1.GetHashCode();
            Assert.AreNotEqual(defaultHash, domainSignatureEffectedHash);

            // Name property isn't a domain signature property and shouldn't affect the hash
            object1.Name = "Foo";
            Assert.AreEqual(domainSignatureEffectedHash, object1.GetHashCode());

            // Changing a domain signature property will impace the hash generated
            object1.Age = 14;
            Assert.AreNotEqual(domainSignatureEffectedHash, object1.GetHashCode());
        }

        [Test]
        public void CannotCompareObjectsWithNoDomainSignatureProperties() {
            ObjectWithOneDomainSignatureProperty object1 = new ObjectWithOneDomainSignatureProperty();
            ObjectWithOneDomainSignatureProperty object2 = new ObjectWithOneDomainSignatureProperty();
            Assert.That(object1, Is.EqualTo(object2));
        }

        [Test]
        public void CanCompareDefaultDomainObjectTypesAsEqual() {
            ObjectWithNoDomainSignatureProperties object1 = new ObjectWithNoDomainSignatureProperties();
            ObjectWithNoDomainSignatureProperties object2 = new ObjectWithNoDomainSignatureProperties();
            Assert.That(object1, Is.Not.EqualTo(object2));
        }

        [Test]
        public void CannotHaveDomainObjectsBeEqualWithNoDomainSignatureProperties() {
            ObjectWithNoDomainSignatureProperties object1 = new ObjectWithNoDomainSignatureProperties();
            ObjectWithNoDomainSignatureProperties object2 = new ObjectWithNoDomainSignatureProperties();
            Assert.That(object1, Is.Not.EqualTo(object2));

            object1.Name = "Foo";
            object2.Name = "Foo";
            Assert.That(object1, Is.Not.EqualTo(object2));
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

        private class ObjectWithNoDomainSignatureProperties : DomainObject
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        private class ObjectWithOneDomainSignatureProperty : DomainObject
        {
            public string Name { get; set; }

            [DomainSignature]
            public int Age { get; set; }
        }

        private class ObjectWithAllDomainSignatureProperty : DomainObject
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

        private class ObjectWithIdenticalTypedProperties : DomainObject
        {
            [DomainSignature]
            public string Name { get; set; }

            [DomainSignature]
            public string Address { get; set; }
        }

        #region ObjectWithComplexProperties

        private class ObjectWithComplexProperties : DomainObject
        {
            [DomainSignature]
            public string Name { get; set; }

            [DomainSignature]
            public AddressBeingDomainSignatureComparble Address { get; set; }

            [DomainSignature]
            public PhoneBeingNotDomainObject Phone { get; set; }
        }

        private class AddressBeingDomainSignatureComparble : DomainObject
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
        }

        #endregion
    }
}
