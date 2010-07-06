using SharpArch.Core.PersistenceSupport;
using Tests.DomainModel.Entities;

namespace Tests.NHibernateTests {
    public interface ITransactionTestProvider {
        IRepository<TestEntity> TestEntityRepository { get; set; }
        string Name { get; }
        void InitTransactionManager();
        void CheckNumberOfEntities(int numberOfEntities);
        void DoCommit(string testEntityName);
        void DoCommitSilenceException(string testEntityName);
        void DoRollback();
        void DoNestedCommit();
        void DoNestedForceRollback();
        void DoNestedInnerForceRollback();
    }
}