using Beis.HelpToGrow.Repositories.Interfaces;
using Beis.HelpToGrow.Persistence;
using Beis.HelpToGrow.Persistence.Models;
using System.Threading.Tasks;

namespace Beis.HelpToGrow.Repositories
{
    public class EligibilityCheckResultRepository : IEligibilityCheckResultRepository
    {
        private readonly HtgVendorSmeDbContext _context;

        public EligibilityCheckResultRepository(HtgVendorSmeDbContext context)
        {
            _context = context;
        }

        public async Task AddCheckResult(eligibility_check_result checkResult)
        {
            await _context.eligibility_check_results.AddAsync(checkResult);
            await _context.SaveChangesAsync();
        }
    }
}