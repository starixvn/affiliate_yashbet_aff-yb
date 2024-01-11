using Fanex.Affiliate.Admin.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public IEnumerable<Product> GetProductList()
        {
            const string SpKey = "Aff_ProductList_Sel";

            using (var objectDb = GetObjectDb(SpKey))
            {
                return objectDb.Query<Product>();
            }
        }
    }
}
