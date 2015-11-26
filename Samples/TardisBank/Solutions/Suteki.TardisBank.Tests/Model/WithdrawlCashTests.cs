namespace Suteki.TardisBank.Tests.Model
{
    using Domain;
    using MediatR;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class WithdrawlCashTests
    {
        [SetUp]
        public void SetUp()
        {
            mediator = new Mock<IMediator>();

            parent = new Parent("Dad", "mike@mike.com", "xxx");
            child = parent.CreateChild("Leo", "leohadlow", "yyy");
            parent.MakePaymentTo(child, 10.00M);

            somebodyElsesParent = new Parent("Not Dad", "jon@jon.com", "zzz");
        }

        [TearDown]
        public void TearDown()
        {
        }

        Parent parent;
        Child child;
        Parent somebodyElsesParent;
        Mock<IMediator> mediator;

        [Test, ExpectedException(typeof (CashWithdrawException), ExpectedMessage = "Not Your Parent")]
        public void Child_shold_not_be_able_to_withdraw_from_some_other_parent()
        {
            child.WithdrawCashFromParent(somebodyElsesParent, 2.30M, "for toys", mediator.Object);
        }


        [Test]
        [SetCulture("en-GB")]
        public void Child_should_be_able_to_withdraw_cash()
        {
            child.WithdrawCashFromParent(parent, 2.30M, "For Toys", mediator.Object);

            child.Account.Balance.ShouldEqual(7.70M);
            child.Account.Transactions[1].Amount.ShouldEqual(-2.30M);
            child.Account.Transactions[1].Description.ShouldEqual("For Toys");

            parent.Messages.Count.ShouldEqual(1);
            parent.Messages[0].Text.ShouldEqual("Leo would like to withdraw \u00A32.30");
        }


        [Test, ExpectedException(typeof (CashWithdrawException),
            ExpectedMessage = "You can not withdraw \u00A312.11 because you only have \u00A310.00 in your account")]
        [SetCulture("en-GB")]
        public void Child_should_not_be_able_to_withdraw_more_than_their_balance()
        {
            child.WithdrawCashFromParent(parent, 12.11M, "For Toys", mediator.Object);
        }


        [Test]
        [SetCulture("en-GB")]
        public void Should_raise_a_SendMessageEvent()
        {
            child.WithdrawCashFromParent(parent, 2.30M, "For Toys", mediator.Object);

            mediator.Verify(m => m.Publish(
                It.Is((Domain.Events.SendMessageEvent ev) =>
                    ev.User == parent && ev.Message == "Leo would like to withdraw \u00A32.30"))
                );
        }
    }
}
