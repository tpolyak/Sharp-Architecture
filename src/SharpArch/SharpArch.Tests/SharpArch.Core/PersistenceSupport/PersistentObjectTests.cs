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
    }
}
