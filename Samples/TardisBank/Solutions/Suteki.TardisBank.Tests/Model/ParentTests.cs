// ReSharper disable InconsistentNaming

namespace Suteki.TardisBank.Tests.Model
{
    using Domain;
    using NUnit.Framework;
    using SharpArch.NHibernate;
    using SharpArch.Testing.NUnit.NHibernate;

    [TestFixture]
    public class ParentTests : RepositoryTestsBase
    {
        int parentId;

        protected override void LoadTestData()
        {
            var parent = new Parent("Mike Hadlow", string.Format("{0}@yahoo.com", "mike"), "yyy");
            Session.Save(parent);
            this.FlushSessionAndEvict(parent);
            parentId = parent.Id;
        }

        [Test]
        public void Should_be_able_to_add_a_child_to_a_parent()
        {
            var linqRepository = new LinqRepository<Parent>(TransactionManager, Session);
            Parent savedParent = linqRepository.Get(parentId);
            savedParent.CreateChild("jim", "jim123", "passw0rd1");
            savedParent.CreateChild("jenny", "jenny123", "passw0rd2");
            savedParent.CreateChild("jez", "jez123", "passw0rd3");
            this.FlushSessionAndEvict(savedParent);


            Parent parent = linqRepository.Get(parentId);
            parent.Children.Count.ShouldEqual(3);

            parent.Children[0].Name.ShouldEqual("jim");
            parent.Children[1].Name.ShouldEqual("jenny");
            parent.Children[2].Name.ShouldEqual("jez");
        }

        [Test]
        public void Should_be_able_to_create_and_retrieve_Parent()
        {
            Parent parent = new LinqRepository<Parent>(TransactionManager, Session).Get(parentId);
            parent.ShouldNotBeNull();
            parent.Name.ShouldEqual("Mike Hadlow");
            parent.UserName.ShouldEqual("mike@yahoo.com");
            parent.Children.ShouldNotBeNull();
        }
    }
}

// ReSharper restore InconsistentNaming
