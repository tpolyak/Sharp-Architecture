// ReSharper disable InconsistentNaming
using System.Linq;
using NUnit.Framework;

namespace Suteki.TardisBank.Tests.Model
{
    using SharpArch.Domain.Events;

    using global::Suteki.TardisBank.Domain;

    [TestFixture]
    public class MessageCountLimitTests
    {
        User user;

        [SetUp]
        public void SetUp()
        {
            //DomainEvents.TurnOff();
            user = new Parent("Mike", "mike@mike.com", "xxx");
        }

        [TearDown]
        public void TearDown()
        {
            //DomainEvents.Reset();
        }


        [Test]
        public void Number_of_messages_should_be_limited()
        {
            for (int i = 0; i < User.MaxMessages; i++)
            {
                user.SendMessage("Message" + i);
            }

            user.Messages.Count.ShouldEqual(User.MaxMessages);

            user.SendMessage("New");
            user.Messages.Count.ShouldEqual(User.MaxMessages);
            user.Messages.First().Text.ShouldEqual("Message1");
            user.Messages.Last().Text.ShouldEqual("New");

            user.SendMessage("New2");
            user.Messages.Count.ShouldEqual(User.MaxMessages);
            user.Messages.First().Text.ShouldEqual("Message2");
            user.Messages.Last().Text.ShouldEqual("New2");
        }
    }
}
// ReSharper restore InconsistentNaming