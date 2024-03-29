﻿using Beis.HelpToGrow.Persistence;
using Beis.HelpToGrow.Persistence.Models;
using Beis.HelpToGrow.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Beis.HelpToGrow.Repositories
{
    public class EnterpriseSizeRepository : IEnterpriseSizeRepository
    {
        
        private readonly HtgVendorSmeDbContext _context;

        public EnterpriseSizeRepository(HtgVendorSmeDbContext context)
        {
            _context = context;
        }

        public async Task<enterprise_size> GetEnterpriseSizeRecord(string size)
        {
            return await _context.enterprise_sizes.SingleOrDefaultAsync(x => EF.Functions.Like(x.enterprise_size_desc, size));
        }

        public async Task<enterprise_size> AddEnterpriseSize(enterprise_size enterpriseSize)
        {
            await _context.enterprise_sizes.AddAsync(enterpriseSize);
            await _context.SaveChangesAsync();
            return enterpriseSize;
        }

        public async Task<enterprise_size> GetEnterpriseSizeRecordById(long id)
        {
            return await _context.enterprise_sizes.SingleOrDefaultAsync(x => x.enterprise_size_id == id);
        }
    }
}
