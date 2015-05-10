// ReSharper disable InconsistentNaming

namespace Suteki.TardisBank.Tests.Model
{
    using SharpArch.NHibernate;
    using SharpArch.Testing.NUnit.NHibernate;

    using NUnit.Framework;

    using global::Suteki.TardisBank.Domain;

    [TestFixture]
    public class MessageTests : RepositoryTestsBase
    {
        private int userId;

        protected override void LoadTestData()
        {
            User user = new Parent("Dad", "mike@mike.com", "xxx");
            NHibernateSession.Current.Save(user);
            this.FlushSessionAndEvict(user);
            userId = user.Id;
        }

        [Test]
        public void Should_be_able_to_add_a_message_to_a_user()
        {
            var parentRepository = new LinqRepository<Parent>();
            User userToTestWith = parentRepository.Get(userId);

            userToTestWith.SendMessage("some message");

            FlushSessionAndEvict(userToTestWith);


            var parent = parentRepository.Get(userId);
            parent.Messages.Count.ShouldEqual(1);
        }
    }
}
// ReSharper restore InconsistentNaming