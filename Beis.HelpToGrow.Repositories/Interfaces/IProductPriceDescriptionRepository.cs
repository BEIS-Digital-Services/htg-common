using System.Threading.Tasks;
using Beis.HelpToGrow.Persistence.Models;

namespace Beis.HelpToGrow.Repositories.Interfaces
{
    public interface IProductPriceDescriptionRepository
    {
        Task<product_price_base_metric_price> GetProductPriceBaseDescription(long productId);
    }
}