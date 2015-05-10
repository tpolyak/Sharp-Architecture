// ReSharper disable InconsistentNaming
using NUnit.Framework;

namespace Suteki.TardisBank.Tests.Model
{
    using SharpArch.Domain.Events;

    using global::Suteki.TardisBank.Domain;
    using global::Suteki.TardisBank.Domain.Events;

    [TestFixture]
    public class UserActivationTests 
    {

        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void ParentShouldNotBeActiveWhenCreated()
        {
            User parent = new Parent("Dad", "mike@mike.com", "xxx");
            parent.IsActive.ShouldBeFalse();
        }

        [Test]
        public void Parent_should_raise_an_event_when_created()
        {
            NewParentCreatedEvent newParentCreatedEvent = null;
            DomainEvents.Register<NewParentCreatedEvent>(e => { newParentCreatedEvent = (NewParentCreatedEvent)e; });
      
            var parent = new Parent("Dad", "mike@mike.com", "xxx").Initialise();

            newParentCreatedEvent.ShouldNotBeNull();
            newParentCreatedEvent.Parent.ShouldBeTheSameAs(parent);
        }

        [Test]
        public void Child_should_be_active_when_created()
        {
            User child = new Parent("Dad", "Mike@mike.com", "xxx").CreateChild("Leo", "leoahdlow", "bbb");
            child.IsActive.ShouldBeTrue();
        }
    }
}
// ReSharper restore InconsistentNaming