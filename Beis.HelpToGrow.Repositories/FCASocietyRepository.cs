using System.Collections.Generic;
using Beis.HelpToGrow.Repositories.Interfaces;
using Beis.HelpToGrow.Persistence;
using Beis.HelpToGrow.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Beis.HelpToGrow.Repositories
{
    public class FCASocietyRepository : IFCASocietyRepository
    {
        private readonly HtgVendorSmeDbContext _context;

        public FCASocietyRepository(HtgVendorSmeDbContext context)
        {
            _context = context;
        }
        
        public async Task<int> GetFCASocietiesCount() =>
            await _context.fcasocieties.CountAsync();

        public async Task AddSocieties(IEnumerable<fcasociety> fcaSocieties)
        {
            await _context.fcasocieties.AddRangeAsync(fcaSocieties);
            await _context.SaveChangesAsync();
        }

        public async Task<fcasociety> GetFCASociety(string fullRegistrationNumber) =>
            await _context
                .fcasocieties
                .FirstOrDefaultAsync(_ => _.full_registration_number == fullRegistrationNumber);

        public async Task<fcasociety> GetFCASocietyByNumber(string societyNumber) =>
            await _context
                .fcasocieties
                .SingleOrDefaultAsync(_ => EF.Functions.Like(_.full_registration_number, societyNumber));
    }
}