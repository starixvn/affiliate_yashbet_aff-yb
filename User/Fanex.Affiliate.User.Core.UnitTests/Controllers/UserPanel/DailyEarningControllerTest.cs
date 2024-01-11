using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel;
using Moq;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers.UserPanel
{
    public class DailyEarningControllerTest
    {
        [Fact]
        public void Index_WhenCall_ReturnCorrectViewModel()
        {
            //Arrange
            var param = new DailyEarningParam
            {
                AffId = Constant.DefaultAffId,
                ProductId = 1,
                MonthDate = new DateTime(2018, 11, 08),
                IsOnlyContent = false,
                IsInternal = false
            };

            var expectProductList = new List<ProductItem>
            {
                new ProductItem
                {
                    ProductId = 1,
                    ProductName = "Sportbook"
                }
            };

            var expectDailyEarningResult = new DailyEarningResult
            {
                MainDataList = new List<DailyEarning>(),
                Total = null
            };

            var statementHistoryRepository = new Mock<IStatementHistoryRepository>();
            statementHistoryRepository.Setup(s => s.GetProductList(param.IsInternal)).Returns(expectProductList);
            statementHistoryRepository.Setup(s => s.GetDailyEarning(param)).Returns(expectDailyEarningResult);
            var controller = CreateController(statementHistoryRepository.Object);

            //Action
            var actionResult = controller.Index(param) as ViewResult;
            var result = actionResult.Model as DailyEarningViewModel;

            var expectSelectedProduct = 1;
            var expectIsKoreanAgentCommGroup = false;


            //Assert
            Assert.Equal(expectSelectedProduct, result.SelectedProduct);
            Assert.Equal(expectIsKoreanAgentCommGroup, result.IsKoreanAgentCommGroup);
            Assert.Equal(expectProductList, result.ProductList);
            Assert.Equal(expectDailyEarningResult, result.Data);
        }

        [Fact]
        public void Index_ProductIdNoValue_AssignCorrectDefaultProductId()
        {
            //Arrange
            var param = new DailyEarningParam
            {
                AffId = Constant.DefaultAffId,
                ProductId = null,
                MonthDate = new DateTime(2018, 11, 08),
                IsOnlyContent = false,
                IsInternal = false
            };

            var productList = new List<ProductItem>();
            var dailyEarningResult = new DailyEarningResult();

            var statementHistoryRepository = new Mock<IStatementHistoryRepository>();
            statementHistoryRepository.Setup(s => s.GetProductList(param.IsInternal)).Returns(productList);
            statementHistoryRepository.Setup(s => s.GetDailyEarning(param)).Returns(dailyEarningResult);
            var controller = CreateController(statementHistoryRepository.Object);

            var expectProductId = 1;

            //Act
            controller.Index(param);

            //Assert
            Assert.Equal(expectProductId, param.ProductId.Value);
        }

        [Fact]
        public void Index_MonthDateNoValue_AssignCorrectDefaultMonthDate()
        {
            //Arrange
            var param = new DailyEarningParam
            {
                AffId = Constant.DefaultAffId,
                ProductId = 1,
                MonthDate = null,
                IsOnlyContent = false,
                IsInternal = false
            };

            var productList = new List<ProductItem>();
            var dailyEarningResult = new DailyEarningResult();

            var statementHistoryRepository = new Mock<IStatementHistoryRepository>();
            statementHistoryRepository.Setup(s => s.GetProductList(param.IsInternal)).Returns(productList);
            statementHistoryRepository.Setup(s => s.GetDailyEarning(param)).Returns(dailyEarningResult);
            var controller = CreateController(statementHistoryRepository.Object);

            //Act
            controller.Index(param);

            //Assert
            Assert.Equal(DateTime.Now.AddDays(-1).Date, param.MonthDate.Value.Date);
        }

        [Fact]
        public void Index_IsOnlyContentHasValue_AssignValueIsTrue()
        {
            //Arrange
            var param = new DailyEarningParam
            {
                AffId = Constant.DefaultAffId,
                ProductId = 1,
                MonthDate = null,
                IsOnlyContent = true,
                IsInternal = false
            };

            var productList = new List<ProductItem>();
            var dailyEarningResult = new DailyEarningResult();

            var statementHistoryRepository = new Mock<IStatementHistoryRepository>();
            statementHistoryRepository.Setup(s => s.GetProductList(param.IsInternal)).Returns(productList);
            statementHistoryRepository.Setup(s => s.GetDailyEarning(param)).Returns(dailyEarningResult);
            var controller = CreateController(statementHistoryRepository.Object);

            //Act
            var actionResult = controller.Index(param) as ViewResult;
            var result = actionResult.Model as DailyEarningViewModel;

            //Assert
            Assert.Null(result.ProductList);
        }

        private static DailyEarningController CreateController(
             IStatementHistoryRepository statementHistoryRepository = null,
             ISessionService sessionService = null,
             ILanguageService languageService = null,
             ICurrencyRepository currencyRepository = null)
        {
            var appSetting = Mock.Of<IAppSetting>();
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakeLanguageService = languageService ?? Mock.Of<ILanguageService>();
            var fakeCurrencyRepository = currencyRepository ?? Mock.Of<ICurrencyRepository>();
            var fakeStatementHistoryRepository = statementHistoryRepository ?? Mock.Of<IStatementHistoryRepository>();
            var controller = new DailyEarningController(appSetting, fakeSessionService, fakeLanguageService, fakeStatementHistoryRepository, fakeCurrencyRepository);

            return MockingHelper.FakeController(controller) as DailyEarningController;
        }
    }
}
