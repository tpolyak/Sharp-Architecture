// ReSharper disable UnusedMember.Global

namespace Tests.SharpArch.NHibernate;

using FluentAssertions;
using global::NHibernate;
using global::SharpArch.Domain.DomainModel;
using global::SharpArch.Domain.PersistenceSupport;
using global::SharpArch.NHibernate;
using Moq;
using Xunit;


public class RepositoryTests
{
    [Fact]
    public void CanCastConcreteLinqRepositoryToInterfaceILinqRepository()
    {
        var session = new Mock<ISession>();
        var transactionManager = new Mock<INHibernateTransactionManager>();
        transactionManager.SetupGet(t => t.Session).Returns(session.Object);
        var concreteRepository = new LinqRepository<MyEntity, int>(transactionManager.Object);

        concreteRepository.Should().BeAssignableTo<ILinqRepository<MyEntity, int>>();
    }
}


public class MyEntity : Entity<int>
{
    public string? Name { get; set; }
}
