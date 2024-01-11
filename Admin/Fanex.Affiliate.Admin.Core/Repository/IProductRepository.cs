using Fanex.Affiliate.Admin.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProductList();
    }
}
