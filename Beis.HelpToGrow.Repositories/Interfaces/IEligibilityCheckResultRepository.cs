using System.Threading.Tasks;
using Beis.HelpToGrow.Persistence.Models;

namespace Beis.HelpToGrow.Repositories.Interfaces
{
    public interface IEligibilityCheckResultRepository
    {
        Task AddCheckResult(eligibility_check_result checkResult);
    }
}