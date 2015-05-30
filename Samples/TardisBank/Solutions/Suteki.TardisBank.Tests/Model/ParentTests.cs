// ReSharper disable InconsistentNaming
using NUnit.Framework;

namespace Suteki.TardisBank.Tests.Model
{
    using SharpArch.NHibernate;
    using SharpArch.Testing.NUnit.NHibernate;

    using global::Suteki.TardisBank.Domain;

    [TestFixture]
    public class ParentTests : RepositoryTestsBase
    {
        private int parentId;

        protected override void LoadTestData()
        {
            var parent = new Parent(name: "Mike Hadlow", userName: string.Format("{0}@yahoo.com", "mike"), password: "yyy");
            NHibernateSession.Current.Save(parent);
            this.FlushSessionAndEvict(parent);
            parentId = parent.Id;
        }

        [Test]
        public void Should_be_able_to_create_and_retrieve_Parent()
        {
            var parent = new LinqRepository<Parent>().Get(parentId);
            parent.ShouldNotBeNull();
            parent.Name.ShouldEqual("Mike Hadlow");
            parent.UserName.ShouldEqual("mike@yahoo.com");
            parent.Children.ShouldNotBeNull();
        }

        [Test]
        public void Should_be_able_to_add_a_child_to_a_parent()
        {
            var linqRepository = new LinqRepository<Parent>();
            var savedParent = linqRepository.Get(parentId);
            savedParent.CreateChild("jim", "jim123", "passw0rd1");
            savedParent.CreateChild("jenny", "jenny123", "passw0rd2");
            savedParent.CreateChild("jez", "jez123", "passw0rd3");
            this.FlushSessionAndEvict(savedParent);


            var parent = linqRepository.Get(parentId);
            parent.Children.Count.ShouldEqual(3);

            parent.Children[0].Name.ShouldEqual("jim");
            parent.Children[1].Name.ShouldEqual("jenny");
            parent.Children[2].Name.ShouldEqual("jez");
        }
    }
}
// ReSharper restore InconsistentNaming