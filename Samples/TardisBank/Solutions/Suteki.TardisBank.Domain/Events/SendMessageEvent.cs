namespace Suteki.TardisBank.Domain.Events
{
    using System;

    using SharpArch.Domain.Events;

    public class SendMessageEvent : IDomainEvent
    {
        public User User { get; private set; }
        public string Message { get; private set; }

        public SendMessageEvent(User user, string message)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            this.User = user;
            this.Message = message;
        }
    }
}