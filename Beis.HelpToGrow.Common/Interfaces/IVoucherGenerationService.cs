using System.Threading.Tasks;
using Beis.HelpToGrow.Common.Config;
using Beis.HelpToGrow.Persistence.Models;
using Microsoft.Extensions.Options;

namespace Beis.HelpToGrow.Common.Interfaces
{
    public interface IVoucherGenerationService
    {
        public Task<string> GenerateVoucher(vendor_company vendorCompany, enterprise enterprise, product product, IOptions<VoucherSettings> voucherSettings,  bool returnExistingVoucherIfExists = true);
        public string GenerateSetCode(int length);
    }
}