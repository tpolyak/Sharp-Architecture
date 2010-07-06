using SharpArch.Testing.NUnit.NHibernate;
using NUnit.Framework;
using Northwind.Core;
using Northwind.Core.DataInterfaces;
using Northwind.Data;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Data.NHibernate;
using System;

namespace Tests.Northwind.Data
{
    [TestFixture]
    [Category("DB Tests")]
    public class ProxyEqualityTests : DatabaseRepositoryTestsBase
    {
        [Test]
        public void ProxyEqualityTest() {
            Territory territory = territoryRepository.Get("31406");
            Region proxiedRegion = territory.RegionBelongingTo;
            Region unproxiedRegion = regionRepository.Get(4);

            Assert.IsTrue(proxiedRegion.Equals(unproxiedRegion));
            Assert.IsTrue(unproxiedRegion.Equals(proxiedRegion));
        }

        private IRepositoryWithTypedId<Territory, string> territoryRepository = 
            new RepositoryWithTypedId<Territory, string>();
        private IRepository<Region> regionRepository = new Repository<Region>();
    }
}
