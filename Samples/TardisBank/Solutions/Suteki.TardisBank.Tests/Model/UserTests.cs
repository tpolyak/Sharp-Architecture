// ReSharper disable InconsistentNaming

namespace Suteki.TardisBank.Tests.Model
{
    using System.Linq;

    using NHibernate.Linq;

    using NUnit.Framework;

    using SharpArch.NHibernate;
    using SharpArch.Testing.NUnit.NHibernate;

    using global::Suteki.TardisBank.Domain;

    [TestFixture]
    public class UserTests : RepositoryTestsBase
    {
        protected override void LoadTestData()
        {
            var mike = new Parent("Mike Hadlow", "mike@yahoo.com", "yyy");
            Session.Save(mike);

            var leo = mike.CreateChild("Leo", "leohadlow", "xxx");
            var yuna = mike.CreateChild("Yuna", "yunahadlow", "xxx");
            Session.Save(leo);
            Session.Save(yuna);
            
            var john = new Parent("John Robinson", "john@gmail.com", "yyy");
            Session.Save(john);

            var jim = john.CreateChild("Jim", "jimrobinson", "xxx");
            Session.Save(jim);

            Session.Flush();
        }

        [Test]
        public void Should_be_able_to_treat_Parents_and_Children_Polymorphically()
        {
                var users = Session.Query<User>().ToArray();

                users.Length.ShouldEqual(5);
            
                users[0].GetType().Name.ShouldEqual("Parent");
                users[1].GetType().Name.ShouldEqual("Child");
                users[2].GetType().Name.ShouldEqual("Child");
                users[3].GetType().Name.ShouldEqual("Parent");
                users[4].GetType().Name.ShouldEqual("Child");
        }
    }
}
// ReSharper restore InconsistentNaming