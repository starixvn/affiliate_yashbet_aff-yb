using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Data;
using Moq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Repository
{
    public class ProductRepositoryTest
    {
        [Fact]
        public void GetProductList_WhenCall_CallCorrectMethodOfObjectDb()
        {
            // Arrange
            var objectDb = new Mock<IObjectDb>();
            var repository = new Mock<ProductRepository> { CallBase = true };
            repository.Setup(r => r.GetObjectDb("Aff_ProductList_Sel", It.IsAny<int>())).Returns(objectDb.Object);

            // Action
            repository.Object.GetProductList();

            // Assert
            objectDb.Verify(obj => obj.Query<Product>(null, null), Times.Once);
        }
    }
}
