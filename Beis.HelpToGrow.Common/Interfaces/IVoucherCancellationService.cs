using Beis.HelpToGrow.Common.Enums;
using System.Threading.Tasks;

namespace Beis.HelpToGrow.Common.Interfaces
{
    public interface IVoucherCancellationService
    {
        Task<CancellationResponse> CancelVoucherFromEmailLink(long enterpriseId, string emailAddress);
        Task<CancellationResponse> CancelVoucherFromVoucherCode(string voucherCode, string vendorRegistration, string vendorAccessCode);
    }
}