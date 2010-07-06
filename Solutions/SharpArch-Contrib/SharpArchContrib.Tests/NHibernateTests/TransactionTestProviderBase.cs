using System;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Data.NHibernate;
using SharpArch.Testing.NUnit;
using SharpArchContrib.Data.NHibernate;
using Tests.DomainModel.Entities;

namespace Tests.NHibernateTests {
    public abstract class TransactionTestProviderBase {
        protected abstract string TestEntityName { get; }
        public IRepository<TestEntity> TestEntityRepository { get; set; }

        protected void InsertTestEntity(string name) {
            var testEntityRepository = new Repository<TestEntity>();
            var testEntity = new TestEntity
                                 {
                                     Name = name
                                 };
            testEntityRepository.SaveOrUpdate(testEntity);
            NHibernateSession.Current.Evict(testEntity);
        }

        public void CheckNumberOfEntities(int numberOfEntities) {
            var testEntityRepository = new Repository<TestEntity>();
            IList<TestEntity> testEntities = testEntityRepository.GetAll();
            testEntities.Count.ShouldEqual(numberOfEntities);
        }

        public virtual void Commit(string testEntityName) {
            DoCommit(testEntityName);
            CheckNumberOfEntities(1);
        }

        public virtual void DoCommit(string testEntityName) {
            InsertTestEntity(testEntityName);
        }

        public virtual void DoCommitSilenceException(string testEntityName) {
            InsertTestEntity(testEntityName);
            throw new Exception("Unknown Issue");
        }

        public virtual void DoRollback() {
            InsertTestEntity(TestEntityName);
            throw new AbortTransactionException();
        }

        public virtual void DoNestedCommit() {
            InsertTestEntity(TestEntityName + "Outer");
            DoCommit(TestEntityName);
        }

        public virtual void DoNestedForceRollback() {
            InsertTestEntity(TestEntityName + "Outer");
            DoCommit(TestEntityName);
            throw new AbortTransactionException();
        }

        public virtual void DoNestedInnerForceRollback() {
            InsertTestEntity(TestEntityName + "Outer");
            DoRollback();
        }
    }
}