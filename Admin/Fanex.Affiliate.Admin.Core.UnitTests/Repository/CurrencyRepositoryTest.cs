using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.Repository;
using Fanex.Data;
using Moq;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Repository
{
    public class CurrencyRepositoryTest
    {
        [Fact]
        public void GetBaseCurrencies_WhenCall_CallCorrectMethodOfObjectDb()
        {
            // Arrange
            var objectDb = new Mock<IObjectDb>();
            var repository = new Mock<CurrencyRepository> { CallBase = true };
            repository.Setup(r => r.GetObjectDb("Aff_BaseCurrency_Sel", It.IsAny<int>())).Returns(objectDb.Object);

            // Action
            repository.Object.GetBaseCurrencies();

            // Assert
            objectDb.Verify(obj => obj.Query<Currency>(null, null), Times.Once);
        }

        [Fact]
        public void GetCurrencies_WhenCall_CallCorrectMethodOfObjectDb()
        {
            // Arrange
            var objectDb = new Mock<IObjectDb>();
            var repository = new Mock<CurrencyRepository> { CallBase = true };
            repository.Setup(r => r.GetObjectDb("Aff_Currency_Sel", It.IsAny<int>())).Returns(objectDb.Object);

            // Action
            repository.Object.GetCurrencies();

            // Assert
            objectDb.Verify(obj => obj.Query<Currency>(null, null), Times.Once);
        }

        [Fact]
        public void GetPaymentCurrencies_WhenCall_CallCorrectMethodOfObjectDb()
        {
            // Arrange
            var objectDb = new Mock<IObjectDb>();
            var repository = new Mock<CurrencyRepository> { CallBase = true };
            repository.Setup(r => r.GetObjectDb("Aff_PaymentCurrency_Sel", It.IsAny<int>())).Returns(objectDb.Object);

            // Action
            repository.Object.GetPaymentCurrencies();

            // Assert
            objectDb.Verify(obj => obj.Query<Currency>(null, null), Times.Once);
        }
    }
}
