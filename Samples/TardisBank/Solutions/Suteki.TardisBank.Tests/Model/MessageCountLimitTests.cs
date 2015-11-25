namespace Suteki.TardisBank.Tests.Model
{
    using System.Linq;
    using Domain;
    using MediatR;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class MessageCountLimitTests
    {
        [SetUp]
        public void SetUp()
        {
            mediator = new Mock<IMediator>();
            user = new Parent("Mike", "mike@mike.com", "xxx");
        }

        [TearDown]
        public void TearDown()
        {
            //DomainEvents.Reset();
        }

        User user;
        Mock<IMediator> mediator;


        [Test]
        public void Number_of_messages_should_be_limited()
        {
            for (var i = 0; i < User.MaxMessages; i++)
            {
                user.SendMessage("Message" + i, mediator.Object);
            }

            user.Messages.Count.ShouldEqual(User.MaxMessages);

            user.SendMessage("New", mediator.Object);
            user.Messages.Count.ShouldEqual(User.MaxMessages);
            user.Messages.First().Text.ShouldEqual("Message1");
            user.Messages.Last().Text.ShouldEqual("New");

            user.SendMessage("New2", mediator.Object);
            user.Messages.Count.ShouldEqual(User.MaxMessages);
            user.Messages.First().Text.ShouldEqual("Message2");
            user.Messages.Last().Text.ShouldEqual("New2");
        }
    }
}
