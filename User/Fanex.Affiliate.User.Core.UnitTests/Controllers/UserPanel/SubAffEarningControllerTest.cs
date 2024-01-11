using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Controllers.UserPanel;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers.UserPanel
{
    public class SubAffEarningControllerTest
    {
        private readonly IAppSetting AppSetting;
        private readonly ISessionService SessionService;
        private readonly ICurrencyRepository CurrencyRepository;
        private readonly ILanguageService LanguageService;
        private readonly IStatementHistoryRepository StatementHistoryRepository;

        public SubAffEarningControllerTest()
        {
            AppSetting = Substitute.For<IAppSetting>();
            SessionService = NSubtituteMockingHelper.FakeSessionService();
            LanguageService = NSubtituteMockingHelper.FakeLanguageService();
            StatementHistoryRepository = Substitute.For<IStatementHistoryRepository>();
            CurrencyRepository = Substitute.For<ICurrencyRepository>();
        }
        [Fact]
        public void Index_WhenCallWith_ReturnCorrectView()
        {
            //Arrange
            StatementHistoryRepository.GetSubAffCommission(Arg.Any<SubAffComParam>()).Returns(new List<SubAffComInfo> {
                new SubAffComInfo
                {
                    UserName = "ABC",
                    Commission = 1000m,
                    SubAffCommission = 500m
                }
            });

            var controller = new SubAffEarningController(AppSetting, SessionService, LanguageService, StatementHistoryRepository, CurrencyRepository);

            //Action
            var result = controller.Index(new SubAffComParam { MonthDate = new DateTime(2020, 12, 08) }) as ViewResult;
            var model = result.Model as IEnumerable<SubAffComViewModel>;

            //Assert
            Assert.Single(model);
            Assert.Equal("~/Views/UserPanel/StatementHistory/SubAffEarning.cshtml", result.ViewName);
        }

        [Fact]
        public void Index_WhenCallWithIsOnlyContent_ReturnCorrectView()
        {
            //Arrange
            StatementHistoryRepository.GetSubAffCommission(Arg.Any<SubAffComParam>()).Returns(new List<SubAffComInfo> {
                new SubAffComInfo
                {
                    UserName = "ABC",
                    Commission = 1000m,
                    SubAffCommission = 500m
                }
            });
            var controller = new SubAffEarningController(AppSetting, SessionService, LanguageService, StatementHistoryRepository, CurrencyRepository);

            //Action
            var result = controller.Index(new SubAffComParam { IsOnlyContent = true, RequestPage = "abc", MonthDate = null }) as ViewResult;
            var model = result.Model as IEnumerable<SubAffComViewModel>;

            //Assert
            Assert.Single(model);
            Assert.Equal("~/Views/UserPanel/StatementHistory/StatementHistoryPartial/_SubAffEarningContent.cshtml", result.ViewName);
        }
    }
}
