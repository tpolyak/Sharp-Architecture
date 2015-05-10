namespace Suteki.TardisBank.Domain.Events
{
    using System;

    using SharpArch.Domain.Events;

    public class NewParentCreatedEvent : IDomainEvent
    {
        public NewParentCreatedEvent(Parent parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            this.Parent = parent;
        }

        public Parent Parent { get; private set; }
    }
}