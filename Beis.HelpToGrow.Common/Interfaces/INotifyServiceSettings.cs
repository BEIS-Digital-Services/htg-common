namespace Beis.HelpToGrow.Common.Interfaces
{
    public interface INotifyServiceSettings
    {
        string NotifyApiKey { get; }
        string TokenRedeemEmailReminder1TemplateId { get; }
        string TokenRedeemEmailReminder2TemplateId { get; }
        string TokenRedeemEmailReminder3TemplateId { get; }
        string VerifyApplicantEmailAddressTemplateId { get; }
        string IssueTokenTemplateId { get; }

        string EmailVerificationUrl { get; }
    }
}