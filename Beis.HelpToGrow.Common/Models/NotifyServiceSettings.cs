using Beis.HelpToGrow.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace Beis.HelpToGrow.Common.Models
{
    public class NotifyServiceSettings : INotifyServiceSettings
    {
        private readonly IConfiguration _configuration;

        public NotifyServiceSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string NotifyApiKey => _configuration["NotifyApiKey"];
        public string TokenRedeemEmailReminder1TemplateId => _configuration["TokenRedeemEmailReminder1TemplateId"];
        public string TokenRedeemEmailReminder2TemplateId => _configuration["TokenRedeemEmailReminder2TemplateId"];
        public string TokenRedeemEmailReminder3TemplateId => _configuration["TokenRedeemEmailReminder3TemplateId"];
        public string VerifyApplicantEmailAddressTemplateId => _configuration["NotifyVerifyEmailTemplateId"];
        public string IssueTokenTemplateId => _configuration["NotifyIssueTokenTemplateId"];

        public string EmailVerificationUrl => _configuration["EmailVerificationUrl"];


      
    }
}