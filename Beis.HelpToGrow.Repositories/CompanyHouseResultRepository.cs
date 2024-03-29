﻿using System.Linq;
using System.Threading.Tasks;
using Beis.HelpToGrow.Repositories.Interfaces;
using Beis.HelpToGrow.Persistence;
using Beis.HelpToGrow.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Beis.HelpToGrow.Repositories
{
    public class CompanyHouseResultRepository : ICompanyHouseResultRepository
    {
        private readonly HtgVendorSmeDbContext _context;

        public CompanyHouseResultRepository(HtgVendorSmeDbContext context)
        {
            _context = context;
        }

        public async Task SaveCompanyHouseResult(companies_house_api_result result)
        {
            await _context.companies_house_api_result.AddAsync(result);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(string companyHouseNumber)
        {
            var existing = await _context.companies_house_api_result.Where(_ => _.company_number.Equals(companyHouseNumber)).ToListAsync();

            return existing.Any();
        }
    }
}