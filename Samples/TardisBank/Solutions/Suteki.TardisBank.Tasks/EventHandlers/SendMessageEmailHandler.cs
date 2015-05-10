namespace Suteki.TardisBank.Tasks.EventHandlers
{
    using System;

    using SharpArch.Domain.Events;

    using Suteki.TardisBank.Domain;
    using Suteki.TardisBank.Domain.Events;

    public class SendMessageEmailHandler : IHandles<SendMessageEvent>
    {
        readonly IEmailService emailService;

        public SendMessageEmailHandler(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public void Handle(SendMessageEvent sendMessageEvent)
        {
            if (sendMessageEvent == null)
            {
                throw new ArgumentNullException("sendMessageEvent");
            }

            if (sendMessageEvent.User is Child)
            {
                // we cannot send email messages to children.
                return;
            }

            var toAddress = sendMessageEvent.User.UserName;
            const string subject = "Message from Tardis Bank";
            var body = sendMessageEvent.Message;

            this.emailService.SendEmail(toAddress, subject, body);
        }
    }
}