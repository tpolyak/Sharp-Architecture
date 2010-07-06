using SharpArch.Core.PersistenceSupport;
using Northwind.Core;
using Rhino.Mocks;
using System.Collections.Generic;

namespace Tests.Northwind.Data.TestDoubles
{
    public class MockTerritoryRepositoryFactory
    {
        public static IRepository<Territory> CreateMockTerritoryRepository() {
            IRepository<Territory> mockedRepository = MockRepository.GenerateMock<IRepository<Territory>>();
            mockedRepository.Expect(mr => mr.GetAll()).Return(CreateTerritories());

            return mockedRepository;
        }

        private static List<Territory> CreateTerritories() {
            List<Territory> territories = new List<Territory>();

            // Create a number of domain object instances here and add them to the list

            return territories;
        }
    }
}
