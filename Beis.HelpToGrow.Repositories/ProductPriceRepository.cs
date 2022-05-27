using Beis.HelpToGrow.Repositories.Interfaces;
using Beis.Htg.VendorSme.Database;
using Beis.Htg.VendorSme.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Beis.HelpToGrow.Repositories
{
    public class ProductPriceRepository : IProductPriceRepository
    {
        private readonly HtgVendorSmeDbContext _context;

        public ProductPriceRepository(HtgVendorSmeDbContext context)
        {
            _context = context;
        }

        public async Task<product_price> GetByProductId(long productId)
        {
            return await _context.product_prices.SingleOrDefaultAsync(x => x.productid == productId);
        }
    }
}
