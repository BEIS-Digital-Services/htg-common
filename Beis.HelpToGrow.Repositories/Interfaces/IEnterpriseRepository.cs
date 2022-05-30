using Beis.HelpToGrow.Persistence.Models;
using System.Threading.Tasks;

namespace Beis.HelpToGrow.Repositories.Interfaces
{
    public interface IEnterpriseRepository
    {
        Task AddEnterprise(enterprise enterprise);
        Task<enterprise> GetEnterprise(long enterpriseId);
        Task<enterprise> GetEnterpriseByCompanyNumber(string companyNumber);

        Task<enterprise> GetEnterpriseByFCANumber(string fcaNumber);
        Task<enterprise> UpdateEnterprise(enterprise enterprise);
    }
}