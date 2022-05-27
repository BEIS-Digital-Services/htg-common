using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beis.HelpToGrow.Common.Interfaces
{
    public interface IEmailClientService
    {
        Task SendEmailAsync(string emailAddress, string templateId, Dictionary<string, object> personalisation);
    }
}