﻿using Beis.HelpToGrow.Persistence.Models;
using System.Threading.Tasks;

namespace Beis.HelpToGrow.Repositories.Interfaces
{
    public interface IProductPriceRepository
    {
        Task<product_price> GetByProductId(long productId);
    }
}