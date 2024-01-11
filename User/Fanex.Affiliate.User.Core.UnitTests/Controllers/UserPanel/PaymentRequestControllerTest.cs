using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.Controllers;
using Fanex.Affiliate.User.Core.CoreService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.Enum;
using Fanex.Affiliate.User.Core.MetalModels.Email;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels.UserPanel;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Controllers
{
    public class PaymentRequestControllerTest
    {
        private static PaymentRequestController CreatePaymentRequestController(ISessionService sessionService = null,
            IAppSetting appSetting = null,
            ILanguageService languageService = null,
            IPaymentService paymentService = null)
        {
            var fakeSessionService = sessionService ?? MockingHelper.GetSessionService();
            var fakePaymentService = paymentService ?? (new Mock<IPaymentService>()).Object;
            var fakeAppsetting = appSetting ?? (new Mock<IAppSetting>()).Object;
            var fakeLanguageService = languageService ?? (new Mock<ILanguageService>()).Object;
            var controller = new PaymentRequestController(fakeSessionService, fakeAppsetting, fakeLanguageService, fakePaymentService);

            return MockingHelper.FakeController(controller) as PaymentRequestController;
        }

        [Fact]
        public void PaymentRequest_GetPaymentList_ReturnPaymentListCorrectly()
        {
            // Arrange
            var paymentList = new List<Payment> { new Payment() };
            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(p => p.GetPaymentList(It.IsAny<PaymentParam>())).Returns(paymentList);

            var controller = CreatePaymentRequestController(paymentService: paymentService.Object);

            var expectedResult = new PaymentViewModel { PaymentList = paymentList };

            // Action
            var actionResult = controller.Index() as ViewResult;
            var result = actionResult.Model as PaymentViewModel;

            // Assert
            Assert.Equal(expectedResult.PaymentList, result.PaymentList);
        }

        [Fact]
        public void PaymentRequest_GetPaymentList_ReturnOutputInfoCorrectly()
        {
            // Arrange
            const decimal ExpectedBalBroughtForward = 100m;
            const decimal ExpectedMinAmount = 10m;
            const string ExpectedPreferredPayment = "PlayerAccount";

            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(p => p.GetPaymentList(It.IsAny<PaymentParam>()))
                .Returns(new List<Payment>())
                .Callback<PaymentParam>(p =>
                {
                    p.BalBroughtForward = ExpectedBalBroughtForward;
                    p.MinAmount = ExpectedMinAmount;
                    p.PreferredPayment = ExpectedPreferredPayment;
                });

            var controller = CreatePaymentRequestController(paymentService: paymentService.Object);

            var expectedResult = new PaymentViewModel
            {
                BalBroughtForward = ExpectedBalBroughtForward,
                MinAmount = ExpectedMinAmount,
                PreferredPayment = ExpectedPreferredPayment
            };

            // Action
            var actionResult = controller.Index() as ViewResult;
            var result = actionResult.Model as PaymentViewModel;

            // Assert
            Assert.Equal(expectedResult.BalBroughtForward, result.BalBroughtForward);
            Assert.Equal(expectedResult.MinAmount, result.MinAmount);
            Assert.Equal(expectedResult.PreferredPayment, result.PreferredPayment);
        }

        [Theory]
        [InlineData(2008)]
        [InlineData(2020)]
        public void GetPaymentListByYear_WhenCall_ReturnCorrectView(int year)
        {
            // Arrange
            const decimal ExpectedBalBroughtForward = 100m;
            const decimal ExpectedMinAmount = 10m;
            const string ExpectedPreferredPayment = "PlayerAccount";

            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(p => p.GetPaymentList(It.IsAny<PaymentParam>()))
                .Returns(new List<Payment>())
                .Callback<PaymentParam>(p =>
                {
                    p.BalBroughtForward = ExpectedBalBroughtForward;
                    p.MinAmount = ExpectedMinAmount;
                    p.PreferredPayment = ExpectedPreferredPayment;
                });

            var controller = CreatePaymentRequestController(paymentService: paymentService.Object);

            // Action
            var partialViewResult = controller.GetPaymentListByYear(year) as PartialViewResult;

            // Assert
            Assert.Equal("~/Views/UserPanel/PaymentRequest/PaymentPartial/_PaymentList.cshtml", partialViewResult.ViewName);
        }

        [Fact]
        public void PaymentRequestPartial_WhenCall_ReturnCorrectView()
        {
            // Arrange
            const decimal ExpectedBalBroughtForward = 100m;
            const decimal ExpectedMinAmount = 10m;
            const string ExpectedPreferredPayment = "PlayerAccount";

            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(p => p.GetPaymentList(It.IsAny<PaymentParam>()))
                .Returns(new List<Payment>())
                .Callback<PaymentParam>(p =>
                {
                    p.BalBroughtForward = ExpectedBalBroughtForward;
                    p.MinAmount = ExpectedMinAmount;
                    p.PreferredPayment = ExpectedPreferredPayment;
                });

            var controller = CreatePaymentRequestController(paymentService: paymentService.Object);

            // Action
            var partialViewResult = controller.PaymentRequestPartial() as PartialViewResult;

            // Assert
            Assert.Equal("~/Views/UserPanel/PaymentRequest/PaymentPartial/_Main.cshtml", partialViewResult.ViewName);
        }

        [Theory]
        [MemberData(nameof(PaymentViewModelData.GetTestCaseForPendingPayment), MemberType = typeof(PaymentViewModelData))]
        public void PaymentRequest_CheckPendingPaymentInPaymentList_PendingPaymentHasCorrectValue(IEnumerable<Payment> paymentList, bool expected)
        {
            // Arrange
            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(p => p.GetPaymentList(It.IsAny<PaymentParam>()))
                .Returns(paymentList);

            var controller = CreatePaymentRequestController(paymentService: paymentService.Object);

            // Action
            var actionResult = controller.Index() as ViewResult;
            var result = actionResult.Model as PaymentViewModel;

            // Assert
            Assert.Equal(expected, result.PendingPayment != null);
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(-10, 0, false)]
        [InlineData(10, 10, true)]
        [InlineData(20, 10, true)]
        [InlineData(20, 0, true)]
        public void PaymentRequest_ValidateAmount_ReturnCorrectResult(decimal balance, decimal minAmount, bool expected)
        {
            // Arrange
            PaymentParam passedParam = null;
            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(p => p.GetPaymentList(It.IsAny<PaymentParam>()))
                .Returns(new List<Payment>())
                .Callback<PaymentParam>(p =>
                {
                    p.BalBroughtForward = balance;
                    p.MinAmount = minAmount;
                    passedParam = p;
                });

            var controller = CreatePaymentRequestController(paymentService: paymentService.Object);
            // Action
            var actionResult = controller.Index() as ViewResult;
            var result = actionResult.Model as PaymentViewModel;

            // Assert
            Assert.Equal(expected, result.IsValidAmount);
        }

        [Theory]
        [InlineData(true, true)]
        public void PaymentRequest_ValidateDateRange_GetResultByEnvironment(bool isLocal, bool expected)
        {
            // Arrange
            var appSetting = Mock.Of<IAppSetting>(s => s.IsLocal == isLocal);
            var controller = CreatePaymentRequestController(appSetting: appSetting);

            // Action
            var actionResult = controller.Index() as ViewResult;
            var result = actionResult.Model as PaymentViewModel;

            // Assert
            Assert.Equal(result.IsValidDate, expected);
        }

        [Fact]
        public void CancelPaymentRequest_InternalLogin_SetResultInfoCorrectly()
        {
            // Arrange
            var sessionService = MockingHelper.GetSessionService();
            sessionService.IsInternal = true;

            var controller = CreatePaymentRequestController(sessionService: sessionService);

            // Action
            var actionResult = controller.CancelPaymentRequest(new PaymentCancelParam()) as JsonResult;
            var result = actionResult.Data as ResultInfo;

            // Assert
            Assert.False(result.IsOk);
            Assert.Equal(result.Message, Lang.ErrAuthorized);
        }

        [Fact]
        public void CancelPaymentRequest_CallRepository_PassParamCorrectly()
        {
            // Arrange
            const long TransId = 1;
            var param = new PaymentCancelParam { TransId = TransId };
            PaymentCancelParam passedParam = null;

            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(p => p.CancelPaymentRequest(It.IsAny<PaymentCancelParam>(), It.IsAny<PaymentEmailParam>()))
                            .Returns(new ResultInfo())
                            .Callback<PaymentCancelParam, PaymentEmailParam>((p, e) => passedParam = p);

            var controller = CreatePaymentRequestController(paymentService: paymentService.Object);

            var expectedResult = JsonConvert.SerializeObject(new PaymentCancelParam
            {
                AffId = Constant.DefaultAffId,
                Language = Constant.DefaultLanguage,
                TransId = TransId,
                Ip = "::1"
            });

            // Action
            controller.CancelPaymentRequest(param);
            var result = JsonConvert.SerializeObject(passedParam);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CancelPaymentRequest_CallRepository_ReturnResultCorrectly()
        {
            // Arrange
            const long TransId = 1;
            var param = new PaymentCancelParam { TransId = TransId };
            var expectedResult = new ResultInfo { IsOk = false, Message = "Failed" };

            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(p => p.CancelPaymentRequest(It.IsAny<PaymentCancelParam>(), It.IsAny<PaymentEmailParam>()))
                            .Returns(expectedResult);

            var controller = CreatePaymentRequestController(paymentService: paymentService.Object);

            // Action
            var actionResult = controller.CancelPaymentRequest(param) as JsonResult;
            var result = actionResult.Data as ResultInfo;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CancelPaymentRequest_CancelSuccessfully_SendEmail()
        {
            // Arrange
            const long TransId = 1;
            PaymentEmailParam emailParam = null;
            var param = new PaymentCancelParam
            {
                AffId = Constant.DefaultAffId,
                TransId = TransId,
                Amount = 100m,
                RequestDate = new DateTime(2018, 6, 12)
            };

            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(p => p.CancelPaymentRequest(It.IsAny<PaymentCancelParam>(), It.IsAny<PaymentEmailParam>()))
                            .Returns(new ResultInfo { IsOk = true })
                            .Callback<PaymentCancelParam, PaymentEmailParam>((p, e) => emailParam = e);

            var appSetting = GetMockAppSetting();

            var controller = CreatePaymentRequestController(appSetting: appSetting, paymentService: paymentService.Object);

            var expectedEmailParam = GetExpectedEmailParam(PaymentRequestType.Cancel);
            expectedEmailParam.PaymentType = 0;
            var expectedResult = JsonConvert.SerializeObject(expectedEmailParam);

            // Action
            controller.CancelPaymentRequest(param);
            var result = JsonConvert.SerializeObject(emailParam);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CancelPaymentRequest_CancelFailed_DoNothing()
        {
            // Arrange
            const long TransId = 1;
            var param = new PaymentCancelParam
            {
                TransId = TransId,
                Amount = 100m,
                RequestDate = new DateTime(2018, 6, 12)
            };
            var resultInfo = new ResultInfo { IsOk = false };

            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(p => p.CancelPaymentRequest(It.IsAny<PaymentCancelParam>(), It.IsAny<PaymentEmailParam>()))
                            .Returns(resultInfo);

            var controller = CreatePaymentRequestController(paymentService: paymentService.Object);

            // Action
            controller.CancelPaymentRequest(param);

        }

        [Fact]
        public async Task CreatePaymentRequest_InternalLogin_DoNoThingAndSetResultInfoCorrectly()
        {
            // Arrange
            var sessionService = MockingHelper.GetSessionService();
            sessionService.IsInternal = true;

            var controller = CreatePaymentRequestController(sessionService: sessionService);

            // Action
            var actionResult = await controller.CreatePaymentRequest(new PaymentCreationParam()) as JsonResult;
            var result = actionResult.Data as ResultInfo;

            // Assert
            Assert.False(result.IsOk);
            Assert.Equal(result.Message, Lang.ErrAuthorized);
        }

        [Fact]
        public async Task CreatePaymentRequest_WhenCall_PassedCreationParamCorrectly()
        {
            // Arrange
            PaymentCreationParam passedParam = null;
            var creationParam = new PaymentCreationParam
            {
                AffId = Constant.DefaultAffId,
                Amount = 100,
                MinAmount = 100,
                BalBroughtForward = 150,
                TransDate = new DateTime(2020, 1, 1)
            };

            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(p => p.CreatePaymentRequest(It.IsAny<PaymentCreationParam>(), It.IsAny<PaymentEmailParam>()))
                .Returns(Task.FromResult(new ResultInfo { IsOk = true }))
                .Callback<PaymentCreationParam, PaymentEmailParam>((p, e) =>
                {
                    passedParam = p;
                });

            var appSetting = GetMockAppSetting();
            var controller = CreatePaymentRequestController(appSetting: appSetting, paymentService: paymentService.Object);

            var expectedParam = new PaymentCreationParam
            {
                AffId = Constant.DefaultAffId,
                Amount = creationParam.Amount,
                Ip = "::1",
                Language = Constant.DefaultLanguage,
                MinAmount = 100m,
                BalBroughtForward = 150m,
                IsLocalTest = true,
                TransDate = creationParam.TransDate
            };
            var expectedResult = JsonConvert.SerializeObject(expectedParam);

            // Action
            await controller.CreatePaymentRequest(creationParam);
            var result = JsonConvert.SerializeObject(passedParam);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task CreatePaymentRequest_AccountIsNotActived_ReturnCheckingResultIntermediately()
        {
            // Arrange
            var creationParam = new PaymentCreationParam
            {
                Amount = 100,
                MinAmount = 100,
                BalBroughtForward = 150
            };
            var checkingActiveResult = new ResultInfo
            {
                IsOk = false,
                Message = "Not Actived"
            };

            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(p => p.CreatePaymentRequest(It.IsAny<PaymentCreationParam>(), It.IsAny<PaymentEmailParam>()))
                .Returns(Task.FromResult(checkingActiveResult));
            var appSetting = Mock.Of<IAppSetting>(ap => ap.IsLocal);

            var controller = CreatePaymentRequestController(appSetting: appSetting, paymentService: paymentService.Object);

            // Action
            var actionResult = await controller.CreatePaymentRequest(creationParam) as JsonResult;
            var result = actionResult.Data as ResultInfo;

            // Assert
            Assert.Equal(checkingActiveResult, result);
        }

        [Fact]
        public async Task CreatePaymentRequest_AccountIsActivedButCreatePaymentRequestFailed_ReturnCreationResultIntermediately()
        {
            // Arrange
            var creationParam = new PaymentCreationParam
            {
                AffId = Constant.DefaultAffId,
                Amount = 100,
                MinAmount = 100,
                BalBroughtForward = 150
            };

            var creationResult = new ResultInfo
            {
                IsOk = false,
                Message = "Create Failed"
            };
            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(p => p.CreatePaymentRequest(It.IsAny<PaymentCreationParam>(), It.IsAny<PaymentEmailParam>()))
                .Returns(Task.FromResult(creationResult));

            var appSetting = Mock.Of<IAppSetting>(ap => ap.IsLocal);

            var controller = CreatePaymentRequestController(appSetting: appSetting, paymentService: paymentService.Object);

            // Action
            var actionResult = await controller.CreatePaymentRequest(creationParam) as JsonResult;
            var result = actionResult.Data as ResultInfo;

            Assert.Equal(creationResult, result);
        }

        [Fact]
        public async Task CreatePaymentRequest_CreateSuccessfully_SendEmailWithCorrectParam()
        {
            // Arrange
            PaymentEmailParam emailParam = null;
            var creationParam = new PaymentCreationParam
            {
                Amount = 100,
                MinAmount = 100,
                BalBroughtForward = 150
            };

            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(p => p.CreatePaymentRequest(It.IsAny<PaymentCreationParam>(), It.IsAny<PaymentEmailParam>()))
                            .Returns(Task.FromResult(new ResultInfo { IsOk = true }))
                            .Callback<PaymentCreationParam, PaymentEmailParam>((p, e) =>
                            {
                                emailParam = e;
                            });

            var appSetting = GetMockAppSetting();
            var controller = CreatePaymentRequestController(appSetting: appSetting, paymentService: paymentService.Object);


            var expectedEmailParam = GetExpectedEmailParam(PaymentRequestType.Create);
            expectedEmailParam.RequestDate = DateTime.MinValue;
            expectedEmailParam.TransId = 0;
            expectedEmailParam.Amount = 0;
            var expectedResult = JsonConvert.SerializeObject(expectedEmailParam);

            // Action
            await controller.CreatePaymentRequest(creationParam);
            var result = JsonConvert.SerializeObject(emailParam);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        private static IAppSetting GetMockAppSetting()
        {
            return Mock.Of<IAppSetting>(ap =>
                ap.IsLocal &&
                ap.GetMailContactByLanguageCode(It.IsAny<string>()) == Constant.DefaultMailContact &&
                ap.MailBcc == string.Empty &&
                ap.MailCc == Constant.DefaultMailCc);
        }

        private PaymentEmailParam GetExpectedEmailParam(PaymentRequestType paymentType)
        {
            return new PaymentEmailParam
            {
                Amount = 100m,
                MailContact = Constant.DefaultMailContact,
                RequestDate = new DateTime(2018, 6, 12),
                RootDomain = Constant.DefaultRootDomain + "/",
                TransId = 1,
                PaymentType = paymentType
            };
        }
    }

    public static class PaymentViewModelData
    {
        public static IEnumerable<object[]> GetTestCaseForPendingPayment()
        {
            yield return new object[]
            {
                new List<Payment> { new Payment { Status = PaymentStatus.Processing } },
                true
            };

            yield return new object[]
            {
                new List<Payment> { new Payment { Status = PaymentStatus.Approved } },
                false
            };

            yield return new object[]
            {
                new List<Payment>
                {
                    new Payment { Status = PaymentStatus.Approved },
                    new Payment { Status = PaymentStatus.Processing }
                },
                true
            };
        }
    }
}