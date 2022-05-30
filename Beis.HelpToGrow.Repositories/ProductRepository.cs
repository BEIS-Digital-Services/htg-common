
using Beis.HelpToGrow.Repositories.Interfaces;
using Beis.HelpToGrow.Persistence;
using Beis.HelpToGrow.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beis.HelpToGrow.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly HtgVendorSmeDbContext _context;

        public ProductRepository(HtgVendorSmeDbContext context)
        {
            _context = context;
        }

        public Task<List<settings_product_type>> ProductTypes()
        {
            return _context.settings_product_types.ToListAsync();
        }

        public async Task<product> GetProductSingle(long id)
        {
            return await _context.products.FirstOrDefaultAsync(t => t.product_id == id);
        }

        public async Task<List<product>> GetProducts()
        {
            return await _context.products.ToListAsync();
        }


        public product GetProductBySku(string productSku, long vendorId)
        {
            var product = _context.products
                .Where(t => t.product_SKU == productSku && t.vendor_id == vendorId)
                .SingleOrDefault();
            return product;
        }
    }
}