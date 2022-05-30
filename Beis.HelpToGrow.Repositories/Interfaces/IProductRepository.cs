using Beis.HelpToGrow.Persistence.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beis.HelpToGrow.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<product> GetProductSingle(long id);
        Task<List<product>> GetProducts();
        Task<List<settings_product_type>> ProductTypes();
        product GetProductBySku(string productSku, long vendorId);
    }
}