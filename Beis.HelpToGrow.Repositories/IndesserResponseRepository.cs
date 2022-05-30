using Beis.HelpToGrow.Repositories.Interfaces;
using Beis.HelpToGrow.Persistence;
using Beis.HelpToGrow.Persistence.Models;
using System.Threading.Tasks;

namespace Beis.HelpToGrow.Repositories
{
    public class IndesserResponseRepository : IIndesserResponseRepository
    {
        private readonly HtgVendorSmeDbContext _context;

        public IndesserResponseRepository(HtgVendorSmeDbContext context)
        {
            _context = context;
        }

        public async Task<indesser_api_call_status> AddIndesserResponse(indesser_api_call_status indesserApiCallStatus)
        {
            await _context.indesser_api_call_statuses.AddAsync(indesserApiCallStatus);
            await _context.SaveChangesAsync();

            return indesserApiCallStatus;
        }
    }
}