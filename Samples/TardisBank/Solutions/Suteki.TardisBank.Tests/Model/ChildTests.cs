// ReSharper disable InconsistentNaming
using NUnit.Framework;

namespace Suteki.TardisBank.Tests.Model
{
    using System;

    using SharpArch.Domain.PersistenceSupport;
    using SharpArch.NHibernate;
    using SharpArch.Testing.NUnit;
    using SharpArch.Testing.NUnit.NHibernate;

    using global::Suteki.TardisBank.Domain;

    [TestFixture]
    public class ChildTests : RepositoryTestsBase
    {
        private int childId;
        private int parentId;
        protected override void LoadTestData()
        {
            var parent = new Parent("Mike Hadlow", "mike@yahoo.com", "yyy");
            NHibernateSession.Current.Save(parent);

            parentId = parent.Id;

            var child = parent.CreateChild("Leo", "leohadlow", "xxx");
            NHibernateSession.Current.Save(child);
            RepositoryTestsHelper.FlushSessionAndEvict(child);
            RepositoryTestsHelper.FlushSessionAndEvict(parent);
            this.childId = child.Id;
        }

        [Test]
        public void Should_be_able_to_create_and_retrieve_a_child()
        {
                var child = new LinqRepository<Child>().Get(childId);
                child.Name.ShouldEqual("Leo");
                child.UserName.ShouldEqual("leohadlow");
                child.ParentId.ShouldEqual(parentId);
                child.Password.ShouldEqual("xxx");
                child.Account.ShouldNotBeNull();
        }

        [Test]
        public void Should_be_able_to_add_schedule_to_account()
        {
            var childRepository = new LinqRepository<Child>();
            var childToTestOn = childRepository.Get(childId);
            childToTestOn.Account.AddPaymentSchedule(DateTime.UtcNow, Interval.Week, 10, "Weekly pocket money");    
            FlushSessionAndEvict(childToTestOn);

            var child = childRepository.Get(childId);
            child.Account.PaymentSchedules[0].Id.ShouldBeGreaterThan(0);
        }

        [Test]
        public void Should_be_able_to_add_transaction_to_account()
        {
            var childRepository = new LinqRepository<Child>();
            var childToTestOn = childRepository.Get(childId);
            childToTestOn.ReceivePayment(10, "Reward");
            FlushSessionAndEvict(childToTestOn);

            var child = childRepository.Get(childId);
            child.Account.Transactions[0].Id.ShouldBeGreaterThan(0);
        }
    }
}
// ReSharper restore InconsistentNaming