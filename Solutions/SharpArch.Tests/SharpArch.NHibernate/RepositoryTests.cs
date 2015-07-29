namespace Tests.SharpArch.NHibernate
{
    using global::NHibernate;
    using NUnit.Framework;

    using global::SharpArch.Domain.PersistenceSupport;
    using global::SharpArch.NHibernate;
    using global::SharpArch.Testing.NUnit;
    using Moq;

    [TestFixture]
    public class RepositoryTests
    {
        [Test]
        public void CanCastConcreteLinqRepositoryToInterfaceILinqRepository()
        {
            Mock<ISession> session = new Mock<ISession>();
            Mock<ITransactionManager> transactionManager = new Mock<ITransactionManager>();

            LinqRepository<MyEntity> concreteRepository = new LinqRepository<MyEntity>(transactionManager.Object, session.Object);

            ILinqRepository<MyEntity> castRepository = concreteRepository as ILinqRepository<MyEntity>;
            castRepository.ShouldNotBeNull();
        }
    }

    public class MyEntity
    {
        private string Name { get; set; }
    }
}