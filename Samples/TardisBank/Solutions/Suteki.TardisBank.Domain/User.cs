namespace Suteki.TardisBank.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SharpArch.Domain.DomainModel;
    using SharpArch.Domain.Events;

    using Suteki.TardisBank.Domain.Events;

    public abstract class User : Entity
    {
        public const int MaxMessages = 20;

        protected User()
        {
        }

        public virtual string Name { get; protected set; }
        public virtual string UserName { get; protected set; }
        public virtual string Password { get; protected set; }
        public virtual bool IsActive { get; protected set; }
        public virtual IList<Message> Messages { get; protected set; }

        protected User(string name, string userName, string password)
        {
            this.Name = name;
            this.UserName = userName;
            this.Password = password;
            this.Messages = new List<Message>();
            this.IsActive = false;
        }

        public virtual void SendMessage(string text)
        {
            this.Messages.Add(new Message(DateTime.Now.Date, text, this));
            this.RemoveOldMessages();

            DomainEvents.Raise(new SendMessageEvent(this, text));
        }

        void RemoveOldMessages()
        {
            if (this.Messages.Count <= MaxMessages) return;

            var oldestMessage = this.Messages.First();
            this.Messages.Remove(oldestMessage);
        }

        public virtual void ReadMessage(int messageId)
        {
            var message = this.Messages.SingleOrDefault(x => x.Id == messageId);
            if (message == null)
            {
                throw new TardisBankException("No message with Id {0} found for user '{1}'", messageId, this.UserName);
            }
            message.Read();
        }

        public virtual void Activate()
        {
            this.IsActive = true;
        }

        public virtual void ResetPassword(string newPassword)
        {
            this.Password = newPassword;
        }
    }
}