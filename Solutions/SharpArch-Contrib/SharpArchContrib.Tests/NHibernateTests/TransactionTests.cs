using System;
using System.Collections.Generic;
using log4net;
using Microsoft.Practices.ServiceLocation;
using NHibernate.Exceptions;
using NUnit.Framework;
using SharpArchContrib.Data.NHibernate;

namespace Tests.NHibernateTests {
    [TestFixture]
    [Category(TestCategories.FileDbTests)]
    public class TransactionTests : TransactionTestsBase {
        private const string TestEntityName = "TransactionTest";
        private List<ITransactionTestProvider> transactionTestProviders = new List<ITransactionTestProvider>();

        public TransactionTests() {
            ServiceLocatorInitializer.Init(typeof(SystemTransactionManager));
            //transactionTestProviders.Add(new SharpArchContrib.PostSharp.NHibernate.SystemTransactionTestProvider());
            //transactionTestProviders.Add(new SharpArchContrib.PostSharp.NHibernate.SystemUnitOfWorkTestProvider());
            transactionTestProviders.Add(
                ServiceLocator.Current.GetInstance<ITransactionTestProvider>("SystemTransactionTestProvider"));
            transactionTestProviders.Add(ServiceLocator.Current.GetInstance<ITransactionTestProvider>("SystemUnitOfWorkTestProvider"));

            ServiceLocatorInitializer.Init(typeof(NHibernateTransactionManager));
            //transactionTestProviders.Add(new SharpArchContrib.PostSharp.NHibernate.NHibernateTransactionTestProvider());
            //transactionTestProviders.Add(new SharpArchContrib.PostSharp.NHibernate.NHibernateUnitOfWorkTestProvider());
            transactionTestProviders.Add(ServiceLocator.Current.GetInstance<ITransactionTestProvider>("NHibernateTransactionTestProvider"));
            transactionTestProviders.Add(ServiceLocator.Current.GetInstance<ITransactionTestProvider>("NHibernateUnitOfWorkTestProvider"));
        }

        protected override void InitializeData() {
            base.InitializeData();
            transactionTestProviders = GenerateTransactionManagers();
        }

        private List<ITransactionTestProvider> GenerateTransactionManagers() {
            foreach (
                ITransactionTestProvider transactionTestProvider in
                    transactionTestProviders) {
                transactionTestProvider.TestEntityRepository = testEntityRepository;
            }

            return transactionTestProviders;
        }

        //Tests call Setup and TearDown manually for each iteration of the loop since
        //we want a clean database for each iteration.  We could use the parameterized
        //test feature of Nunit 2.5 but, unfortunately that doesn't work with all test runners (e.g. Resharper)

        private bool PerformMultipleOperations(ITransactionTestProvider transactionTestProvider) {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoCommit(TestEntityName);
            transactionTestProvider.CheckNumberOfEntities(1);

            transactionTestProvider.DoCommit(TestEntityName + "1");
            transactionTestProvider.CheckNumberOfEntities(2);

            return true;
        }

        private bool PerformMultipleOperationsRollbackFirst(ITransactionTestProvider transactionTestProvider) {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoRollback();
            transactionTestProvider.CheckNumberOfEntities(0);

            transactionTestProvider.DoCommit(TestEntityName);
            transactionTestProvider.CheckNumberOfEntities(1);

            return true;
        }

        private bool PerformMultipleOperationsRollbackLast(ITransactionTestProvider transactionTestProvider) {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoCommit(TestEntityName + "1");
            transactionTestProvider.CheckNumberOfEntities(1);

            transactionTestProvider.DoRollback();
            transactionTestProvider.CheckNumberOfEntities(1);

            return true;
        }

        private bool PerformNestedCommit(ITransactionTestProvider transactionTestProvider) {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoNestedCommit();
            transactionTestProvider.CheckNumberOfEntities(2);

            return true;
        }

        private bool PerformNestedForceRollback(ITransactionTestProvider transactionTestProvider) {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoNestedForceRollback();
            transactionTestProvider.CheckNumberOfEntities(0);

            return true;
        }

        private bool PerformNestedInnerForceRollback(ITransactionTestProvider transactionTestProvider) {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoNestedInnerForceRollback();
            transactionTestProvider.CheckNumberOfEntities(0);

            return true;
        }

        private bool PerformRollback(ITransactionTestProvider transactionTestProvider) {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoRollback();
            transactionTestProvider.CheckNumberOfEntities(0);

            return true;
        }

        private bool PerformRollsbackOnException(ITransactionTestProvider transactionTestProvider) {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoCommit(TestEntityName);
            transactionTestProvider.CheckNumberOfEntities(1);

            Assert.Throws<GenericADOException>(() => transactionTestProvider.DoCommit(TestEntityName));
            transactionTestProvider.CheckNumberOfEntities(1);

            return true;
        }

        private bool PerformRollsbackOnExceptionWithSilentException(ITransactionTestProvider transactionTestProvider) {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoCommit(TestEntityName);
            transactionTestProvider.CheckNumberOfEntities(1);

            transactionTestProvider.DoCommitSilenceException(TestEntityName);
            transactionTestProvider.CheckNumberOfEntities(1);

            return true;
        }

        private bool PerformSingleOperation(ITransactionTestProvider transactionTestProvider) {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoCommit(TestEntityName);
            transactionTestProvider.CheckNumberOfEntities(1);

            return true;
        }

        public void PerformTest(string testName, Func<ITransactionTestProvider, bool> function) {
            ILog logger = LogManager.GetLogger(GetType());
            logger.Debug(string.Format("Starting test {0}", testName));
            try {
                foreach (ITransactionTestProvider transactionTestProvider in transactionTestProviders) {
                    SetUp();
                    try {
                        logger.Debug(string.Format("Transaction Provider: {0}", transactionTestProvider.Name));
                        try {
                            function(transactionTestProvider);
                        }
                        catch (Exception err) {
                            logger.Debug(err);
                            throw;
                        }
                        finally {
                            logger.Debug(string.Format("*** Completed Work With Transaction Provider: {0}",
                                                       transactionTestProvider.Name));
                        }
                    }

                    finally {
                        TearDown();
                    }
                }
            }
            finally {
                logger.Debug(string.Format("*** Completed test {0}", testName));
            }
        }

        [Test]
        public void MultipleOperations() {
            PerformTest("MultipleOperations", testProvider => PerformMultipleOperations(testProvider));
        }

        [Test]
        public void MultipleOperationsRollbackFirst() {
            PerformTest("MultipleOperationsRollbackFirst",
                        testProvider => PerformMultipleOperationsRollbackFirst(testProvider));
        }

        [Test]
        public void MultipleOperationsRollbackLast() {
            PerformTest("MultipleOperationsRollbackLast",
                        testProvider => PerformMultipleOperationsRollbackLast(testProvider));
        }

        [Test]
        public void NestedCommit() {
            PerformTest("NestedCommit", testProvider => PerformNestedCommit(testProvider));
        }

        [Test]
        public void NestedForceRollback() {
            PerformTest("NestedForceRollback", testProvider => PerformNestedForceRollback(testProvider));
        }

        [Test]
        public void NestedInnerForceRollback() {
            PerformTest("NestedInnerForceRollback", testProvider => PerformNestedInnerForceRollback(testProvider));
        }

        [Test]
        public void Rollback() {
            PerformTest("Rollback", testProvider => PerformRollback(testProvider));
        }

        [Test]
        public void RollsbackOnException() {
            PerformTest("RollsbackOnException", testProvider => PerformRollsbackOnException(testProvider));
        }

        [Test]
        public void RollsbackOnExceptionWithSilentException() {
            PerformTest("RollsbackOnExceptionWithSilentException",
                        testProvider => PerformRollsbackOnExceptionWithSilentException(testProvider));
        }

        [Test]
        public void SingleOperation() {
            PerformTest("SingleOperation", testProvider => PerformSingleOperation(testProvider));
        }
    }
}