using NUnit.Framework;
using Northwind.Wcf;
using Northwind.Core;
using SharpArch.Core.PersistenceSupport;
using Rhino.Mocks;
using System.Collections.Generic;
using SharpArch.Testing;
using Northwind.Core.Organization;
using Northwind.Wcf.Dtos;
using NUnit.Framework.SyntaxHelpers;

namespace Tests.Northwind.Wcf
{
    [TestFixture]
    public class TerritoriesWcfServiceTests
    {
        /// <summary>
        /// This is testing the behavior of the logic within the WCF service class; it's not actually 
        /// invoking a WCF service or testing integration but focusing on the logic within the service
        /// itself.
        /// </summary>
        [Test]
        public void CanGetTerritories() {
            ITerritoriesWcfService territoriesWcfService = 
                new TerritoriesWcfService(CreateMockTerritoryRepository());

            IList<TerritoryDto> territoryDtos = territoriesWcfService.GetTerritories();

            Assert.That(territoryDtos.Count, Is.EqualTo(2));
        }

        private IRepository<Territory> CreateMockTerritoryRepository() {
            MockRepository mocks = new MockRepository();

            IRepository<Territory> mockedRepository = mocks.StrictMock<IRepository<Territory>>();
            Expect.Call(mockedRepository.GetAll())
                .Return(CreateTerritories());

            IDbContext mockedDbContext = mocks.StrictMock<IDbContext>();
            SetupResult.For(mockedRepository.DbContext).Return(mockedDbContext);
            
            mocks.Replay(mockedRepository);

            return mockedRepository;
        }

        /// <summary>
        /// This creates "shallow" territories; there's no need to build out the remainder of the 
        /// model as there are other tests which confirm the DTO creation process.
        /// </summary>
        private List<Territory> CreateTerritories() {
            List<Territory> territories = new List<Territory>();

            Territory territory1 = new Territory();
            EntityIdSetter.SetIdOf<string>(territory1, "08837");
            territory1.Description = "Edison";
            territories.Add(territory1);

            Territory territory2 = new Territory();
            EntityIdSetter.SetIdOf<string>(territory2, "00042");
            territory2.Description = "Galactic";
            territories.Add(territory2);

            return territories;
        }
    }
}
