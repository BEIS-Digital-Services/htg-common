using System.Threading.Tasks;
using Beis.HelpToGrow.Persistence.Models;

namespace Beis.HelpToGrow.Common.Interfaces
{
    public interface IVoucherGenerationService
    {
        public Task<string> GenerateVoucher(vendor_company vendorCompany, enterprise enterprise, product product);
        public string GenerateSetCode(int length);
    }
}