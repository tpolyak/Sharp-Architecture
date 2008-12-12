using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Northwind.Core;
using NUnit.Framework.SyntaxHelpers;
using SharpArch.Core;

namespace Tests.Northwind.Core
{
    [TestFixture]
    public class RegionTests
    {
        [Test]
        public void CanCreateRegion() {
            string regionDesc = "Northwest";
            RegionWithPublicConstructor region = new RegionWithPublicConstructor(regionDesc);
            Assert.That(region.Description, Is.EqualTo(regionDesc));
        }
    }
}
