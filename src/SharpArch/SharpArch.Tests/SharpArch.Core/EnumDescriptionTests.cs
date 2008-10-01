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
    public class EnumDescriptionTests
    {
        [Test]
        public void CanRetrieveEnumTextAndDescription() {
            // Simply uses the enum value itself as the text representation by default
            Assert.That(
                EnumDescription.TextRepresentation.GetTextRepresentationOf(OrderStatus.Delivered),
                Is.EqualTo("Delivered"));
            Assert.That(
                EnumDescription.TextRepresentation.GetDescriptionOf(OrderStatus.Delivered),
                Is.EqualTo(String.Empty));

            Assert.That(
                EnumDescription.TextRepresentation.GetTextRepresentationOf(OrderStatus.InventoryDepleted),
                Is.EqualTo("Out of Stock"));
            Assert.That(
                EnumDescription.TextRepresentation.GetDescriptionOf(OrderStatus.InventoryDepleted),
                Is.EqualTo(String.Empty));

            Assert.That(
                EnumDescription.TextRepresentation.GetTextRepresentationOf(OrderStatus.InventoryComing),
                Is.EqualTo("More in Stock Soon"));
            Assert.That(
                EnumDescription.TextRepresentation.GetDescriptionOf(OrderStatus.InventoryComing),
                Is.EqualTo("An order for more items has been placed with the supplier"));
        }

        private enum OrderStatus
        {
            Delivered,
            [EnumDescription.TextRepresentation("Out of Stock")]
            InventoryDepleted,
            [EnumDescription.TextRepresentation("More in Stock Soon", 
                "An order for more items has been placed with the supplier")]
            InventoryComing
        }
    }
}
