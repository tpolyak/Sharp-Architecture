namespace Suteki.TardisBank.Tasks.EventHandlers
{
    using System;

    using SharpArch.Domain.Events;

    using Suteki.TardisBank.Domain;
    using Suteki.TardisBank.Domain.Events;

    public class SendRegistrationEmailHandler : IHandles<NewParentCreatedEvent>
    {
        readonly IEmailService emailService;

        public SendRegistrationEmailHandler(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public void Handle(NewParentCreatedEvent newParentCreatedEvent)
        {
            if (newParentCreatedEvent == null)
            {
                throw new ArgumentNullException("newParentCreatedEvent");
            }
            if (string.IsNullOrWhiteSpace(newParentCreatedEvent.Parent.ActivationKey))
            {
                throw new TardisBankException("Parent does not have an activation key");
            }

            var toAddress = newParentCreatedEvent.Parent.UserName;
            var subject = "Welcome to Tardis Bank";
            var url = "http://tardisbank.com/User/Activate/" + newParentCreatedEvent.Parent.ActivationKey;
            var body = string.Format(emailBodyTemplate, url);

            this.emailService.SendEmail(toAddress, subject, body);
        }

        const string emailBodyTemplate = 
@"Please click here to activate your account: {0} (or copy and Paste this URL into your browser)";
    }
}