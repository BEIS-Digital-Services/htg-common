using System.Threading.Tasks;
using Beis.HelpToGrow.Persistence.Models;

namespace Beis.HelpToGrow.Repositories.Interfaces
{
    public interface ICompanyHouseResultRepository
    {
        Task SaveCompanyHouseResult(companies_house_api_result result);
        Task<bool> Exists(string companyHouseNumber);
    }
}