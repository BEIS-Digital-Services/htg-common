using Beis.HelpToGrow.Repositories.Interfaces;
using Beis.HelpToGrow.Persistence;
using Beis.HelpToGrow.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beis.HelpToGrow.Repositories
{
    public class VendorCompanyRepository : IVendorCompanyRepository
    {
        private readonly HtgVendorSmeDbContext _context;

        public VendorCompanyRepository(HtgVendorSmeDbContext context)
        {
            _context = context;
        }

        public async Task<vendor_company> GetVendorCompanySingle(long id)
        {
            return await _context.vendor_companies.FirstOrDefaultAsync(t => t.vendorid == id);
        }

        public async Task<List<vendor_company>> GetVendorCompanies()
        {
            return await _context.vendor_companies.ToListAsync();
        }

        public vendor_company GetVendorCompanyByRegistration(string registrationId)
        {

            var vendorCompany = _context.vendor_companies
                .Where(t => t.registration_id == registrationId)
                .SingleOrDefault();

            return vendorCompany;

        }
    }
}