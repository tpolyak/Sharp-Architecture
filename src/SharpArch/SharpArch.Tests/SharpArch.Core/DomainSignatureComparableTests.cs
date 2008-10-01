using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpArch.Core;
using NUnit.Framework.SyntaxHelpers;

namespace Tests.SharpArch.Core
{
    [TestFixture]
    public class DomainSignatureComparableTests
    {
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
            Assert.That(object1, Is.Not.EqualTo(object2));

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
            Assert.That(object1, Is.Not.EqualTo(object2));

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
            Assert.That(object1, Is.Not.EqualTo(object2));

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

        private class ObjectWithNoDomainSignatureProperties : DomainSignatureComparable
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        private class ObjectWithOneDomainSignatureProperty : DomainSignatureComparable
        {
            public string Name { get; set; }

            [DomainSignature]
            public int Age { get; set; }
        }

        private class ObjectWithAllDomainSignatureProperty : DomainSignatureComparable
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
    }
}
