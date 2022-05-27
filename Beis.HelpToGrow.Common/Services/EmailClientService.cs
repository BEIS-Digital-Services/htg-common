using Beis.HelpToGrow.Common.Interfaces;
using Notify.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Beis.HelpToGrow.Common.Interfaces;

namespace Beis.HelpToGrow.Common.Services
{
    public class EmailClientService : IEmailClientService
    {
        private readonly INotifyServiceSettings _notificationServiceSettings;

        public EmailClientService(INotifyServiceSettings notifyServiceSettings)
        {
            _notificationServiceSettings = notifyServiceSettings ?? throw new ArgumentException($"Missing {nameof(notifyServiceSettings)}");
        }

        public async Task SendEmailAsync(string emailAddress, string templateId, Dictionary<string, object> personalisation)
        {
            var client = new NotificationClient(_notificationServiceSettings.NotifyApiKey);

            await client.SendEmailAsync(emailAddress, templateId, personalisation);
        }
    }
}