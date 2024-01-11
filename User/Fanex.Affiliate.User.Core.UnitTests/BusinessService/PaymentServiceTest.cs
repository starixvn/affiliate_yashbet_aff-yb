using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.Enum;
using Fanex.Affiliate.User.Core.MetalModels.Email;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.BusinessService
{
    public class PaymentServiceTest
    {
        [Fact]
        public void GetPaymentList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var param = new PaymentParam();
            var paymentRepository = new Mock<IPaymentRepository>();
            paymentRepository.Setup(p => p.GetPaymentList(param)).Returns(new List<Payment> { new Payment() });
            var service = CreatePaymentService(paymentRepository: paymentRepository.Object);

            // Action
            var result = service.GetPaymentList(param);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void CancelPaymentRequest_WhenCall_AssignValueForParamCorrectly()
        {
            // Arrange
            var paymentParam = new PaymentCancelParam { AffId = Constant.DefaultAffId, IsInternal = false };
            var emailParam = new PaymentEmailParam();
            PaymentEmailParam passedEmailParam = null;

            var paymentRepository = new Mock<IPaymentRepository>();
            paymentRepository.Setup(p => p.CancelPaymentRequest(paymentParam)).Returns(new ResultInfo { IsOk = true });

            var userInfo = new UserInfo
            {
                UserName = "usernametest",
                AffId = Constant.DefaultAffId,
                Email = Constant.DefaultMailUser
            };
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(a => a.GetUserInfoById(paymentParam.AffId, paymentParam.IsInternal)).Returns(userInfo);

            var emailService = new Mock<IEmailService>();
            emailService.Setup(e => e.SendPaymentEmail(It.IsAny<PaymentEmailParam>())).Callback<PaymentEmailParam>(p => passedEmailParam = p);

            var service = CreatePaymentService(
                paymentRepository: paymentRepository.Object,
                accountRepository: accountRepository.Object,
                emailService: emailService.Object);

            // Action
            service.CancelPaymentRequest(paymentParam, emailParam);

            // Assert
            Assert.Equal("Cancelled", paymentParam.Status);
            Assert.Equal(PaymentRequestType.Cancel, emailParam.PaymentType);
            Assert.Equal(userInfo.Email, emailParam.MailTo);
            Assert.Equal(JsonConvert.SerializeObject(userInfo), JsonConvert.SerializeObject(passedEmailParam.User));
        }

        [Theory]
        [InlineData(false, null)]
        [InlineData(true, null)]
        public void CancelPaymentRequest_WhenCall_ReturnCorrectResult(bool isSuccessful, string message)
        {
            // Arrange
            var paymentParam = new PaymentCancelParam { AffId = Constant.DefaultAffId, IsInternal = false };
            var emailParam = new PaymentEmailParam();

            var paymentRepository = new Mock<IPaymentRepository>();
            paymentRepository.Setup(p => p.CancelPaymentRequest(paymentParam)).Returns(new ResultInfo { IsOk = isSuccessful });

            UserInfo userInfo = null;
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(a => a.GetUserInfoById(paymentParam.AffId, paymentParam.IsInternal)).Returns(userInfo);

            var service = CreatePaymentService(
                paymentRepository: paymentRepository.Object,
                accountRepository: accountRepository.Object);

            // Action
            var result = service.CancelPaymentRequest(paymentParam, emailParam);

            // Assert
            Assert.Equal(isSuccessful, result.IsOk);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void CancelPaymentRequest_CancelSuccessfullyButUserInfoIsNull_DoNotSendEmail()
        {
            // Arrange
            // Arrange
            var paymentParam = new PaymentCancelParam { AffId = Constant.DefaultAffId, IsInternal = false };
            var emailParam = new PaymentEmailParam();

            var paymentRepository = new Mock<IPaymentRepository>();
            paymentRepository.Setup(p => p.CancelPaymentRequest(paymentParam)).Returns(new ResultInfo { IsOk = true });

            UserInfo userInfo = null;
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(a => a.GetUserInfoById(paymentParam.AffId, paymentParam.IsInternal)).Returns(userInfo);

            var emailService = new Mock<IEmailService>();

            var service = CreatePaymentService(
                paymentRepository: paymentRepository.Object,
                accountRepository: accountRepository.Object,
                emailService: emailService.Object);

            // Action
            service.CancelPaymentRequest(paymentParam, emailParam);

            // Assert
            emailService.Verify(e => e.SendPaymentEmail(It.IsAny<PaymentEmailParam>()), Times.Never());
        }

        [Fact]
        public async Task CreatePaymentRequest_InvalidAmount_ReturnCorrectResultInfo()
        {
            // Arrange
            var param = new PaymentCreationParam
            {
                IsLocalTest = true,
                Amount = 100m,
                MinAmount = 100m,
                BalBroughtForward = 90m
            };

            var service = CreatePaymentService();

            // Action
            var result = await service.CreatePaymentRequest(param, new PaymentEmailParam());

            // Assert
            Assert.False(result.IsOk);
            Assert.Equal(Error.PaymentAmountInvalid, result.Message);
        }

        [Fact]
        public async Task CreatePaymentRequest_ValidParamButCreatePaymentFailed_ReturnCorrectResultInfo()
        {
            // Arrange
            var param = CreateValidPaymentCreationParam();
            const string ErrorMessage = "Create Payment Failed";

            var userInfo = CreateUserInfo();
            var accountInfo = CreateAccountInfo();
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(a => a.GetUserInfoById(param.AffId, param.IsInternal)).Returns(userInfo);
            accountRepository.Setup(a => a.GetAccountInfo(param.AffId)).Returns(accountInfo);

            var accountService = new Mock<IAccountService>();
            accountService.Setup(a => a.CheckAndUnlockUserIfLocked(accountInfo.PlayerUsername)).ReturnsAsync(new ResultInfo { IsOk = true });
            var paymentRepository = new Mock<IPaymentRepository>();
            paymentRepository.Setup(p => p.CreatePaymentRequest(It.IsAny<PaymentRequestInfo>())).Returns(new ResultInfo { Message = ErrorMessage });

            var service = CreatePaymentService(paymentRepository: paymentRepository.Object, accountRepository: accountRepository.Object, accountService: accountService.Object);

            // Action
            var result = await service.CreatePaymentRequest(param, new PaymentEmailParam());

            // Assert
            Assert.False(result.IsOk);
            Assert.Equal(ErrorMessage, result.Message);
        }


        [Fact]
        public async Task CreatePaymentRequest_ValidParamAndSendEmail_ReturnCorrectResultAndEmailParamAssignCorrectly()
        {
            // Arrange
            PaymentEmailParam passedEmailParam = null;
            var param = CreateValidPaymentCreationParam();
            PaymentRequestInfo paymentRequest = null;

            var paymentRepository = new Mock<IPaymentRepository>();
            paymentRepository.Setup(p => p.CreatePaymentRequest(It.IsAny<PaymentRequestInfo>()))
                .Callback<PaymentRequestInfo>(p =>
                {
                    p.TransId = param.TransId;
                    paymentRequest = p;
                })
                .Returns(new ResultInfo { IsOk = true });

            var userInfo = CreateUserInfo();
            var accountInfo = CreateAccountInfo();
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(a => a.GetUserInfoById(param.AffId, param.IsInternal)).Returns(userInfo);
            accountRepository.Setup(a => a.GetAccountInfo(param.AffId)).Returns(accountInfo);

            var emailService = new Mock<IEmailService>();
            emailService.Setup(e => e.SendPaymentEmail(It.IsAny<PaymentEmailParam>())).Callback<PaymentEmailParam>(p => passedEmailParam = p);

            var accountService = new Mock<IAccountService>();
            accountService.Setup(a => a.CheckAndUnlockUserIfLocked(accountInfo.PlayerUsername)).ReturnsAsync(new ResultInfo { IsOk = true });

            var service = CreatePaymentService(
                accountRepository: accountRepository.Object,
                paymentRepository: paymentRepository.Object,
                emailService: emailService.Object,
                accountService: accountService.Object);

            var expectedEmailParam = new PaymentEmailParam
            {
                User = userInfo,
                PaymentType = PaymentRequestType.Create,
                MailTo = userInfo.Email,
                TransId = param.TransId,
                Amount = param.Amount,
                RequestDate = param.TransDate.Value
            };

            // Action
            var result = await service.CreatePaymentRequest(param, new PaymentEmailParam());

            // Assert
            Assert.True(result.IsOk);
            Assert.Equal(JsonConvert.SerializeObject(expectedEmailParam), JsonConvert.SerializeObject(passedEmailParam));
        }

        [Fact]
        public async Task CreatePaymentRequest_ValidParamHasTransDateIsNullAndSendEmail_ReturnCorrectResultAndEmailParamAssignCorrectly()
        {
            // Arrange
            PaymentEmailParam passedEmailParam = null;
            var param = CreatePaymentCreationParamHasTransDateIsNull();
            PaymentRequestInfo paymentRequest = null;

            var paymentRepository = new Mock<IPaymentRepository>();
            paymentRepository.Setup(p => p.CreatePaymentRequest(It.IsAny<PaymentRequestInfo>()))
                .Callback<PaymentRequestInfo>(p =>
                {
                    p.TransId = param.TransId;
                    paymentRequest = p;
                })
                .Returns(new ResultInfo { IsOk = true });

            var userInfo = CreateUserInfo();
            var accountInfo = CreateAccountInfo();
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(a => a.GetUserInfoById(param.AffId, param.IsInternal)).Returns(userInfo);
            accountRepository.Setup(a => a.GetAccountInfo(param.AffId)).Returns(accountInfo);

            var emailService = new Mock<IEmailService>();
            emailService.Setup(e => e.SendPaymentEmail(It.IsAny<PaymentEmailParam>())).Callback<PaymentEmailParam>(p => passedEmailParam = p);

            var accountService = new Mock<IAccountService>();
            accountService.Setup(a => a.CheckAndUnlockUserIfLocked(accountInfo.PlayerUsername)).ReturnsAsync(new ResultInfo { IsOk = true });

            var service = CreatePaymentService(
                accountRepository: accountRepository.Object,
                paymentRepository: paymentRepository.Object,
                emailService: emailService.Object,
                accountService: accountService.Object);

            // Action
            var result = await service.CreatePaymentRequest(param, new PaymentEmailParam());

            // Assert
            Assert.True(result.IsOk);
            Assert.NotEqual(param.TransDate, passedEmailParam.RequestDate);
        }

        [Fact]
        public async Task CreatePaymentRequest_ValidParamButNotSendEmail_ReturnCorrectResultButEmailParamNotAssign()
        {
            // Arrange
            PaymentEmailParam passedEmailParam = null;
            var param = CreateValidPaymentCreationParam();
            PaymentRequestInfo paymentRequest = null;

            var paymentRepository = new Mock<IPaymentRepository>();
            paymentRepository.Setup(p => p.CreatePaymentRequest(It.IsAny<PaymentRequestInfo>()))
                .Callback<PaymentRequestInfo>(p =>
                {
                    p.TransId = param.TransId;
                    paymentRequest = p;
                })
                .Returns(new ResultInfo { IsOk = true });

            var accountInfo = CreateAccountInfo();
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(a => a.GetAccountInfo(param.AffId)).Returns(accountInfo);

            var emailService = new Mock<IEmailService>();
            emailService.Setup(e => e.SendPaymentEmail(It.IsAny<PaymentEmailParam>())).Callback<PaymentEmailParam>(p => passedEmailParam = p);

            var accountService = new Mock<IAccountService>();
            accountService.Setup(a => a.CheckAndUnlockUserIfLocked(accountInfo.PlayerUsername)).ReturnsAsync(new ResultInfo { IsOk = true });

            var service = CreatePaymentService(
                accountRepository: accountRepository.Object,
                paymentRepository: paymentRepository.Object,
                emailService: emailService.Object,
                accountService: accountService.Object);

            // Action
            var result = await service.CreatePaymentRequest(param, new PaymentEmailParam());

            // Assert
            Assert.True(result.IsOk);
            Assert.Null(passedEmailParam);
        }

        [Fact]
        public void GetPaymentMethods_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var paymentRepository = new Mock<IPaymentRepository>();
            paymentRepository.Setup(p => p.GetPaymentMethods()).Returns((new List<PaymentMethodInfo> { new PaymentMethodInfo { PaymentId = "123", PaymentName = "Test" } }, 1));
            var service = CreatePaymentService(paymentRepository: paymentRepository.Object);

            // Action
            var result = service.GetPaymentMethods();

            // Assert
            Assert.Single(result);
        }

        private static UserInfo CreateUserInfo()
        {
            return new UserInfo
            {
                UserName = "usernametest",
                AffId = Constant.DefaultAffId,
                Email = Constant.DefaultMailUser
            };
        }

        private AccountInformation CreateAccountInfo()
            => new AccountInformation
            {
                Address = "abc",
                AffCurrency = "USD",
                AffId = 1,
                City = "ab",
                PlayerUsername = "username"
            };

        private PaymentCreationParam CreateValidPaymentCreationParam()
        {
            return new PaymentCreationParam
            {
                IsLocalTest = true,
                Amount = 100m,
                MinAmount = 100m,
                BalBroughtForward = 1000m,
                AffId = Constant.DefaultAffId,
                Language = Constant.DefaultLanguage,
                TransId = 123,
                TransDate = new DateTime(2018, 12, 28)
            };
        }
        private PaymentCreationParam CreatePaymentCreationParamHasTransDateIsNull()
        {
            return new PaymentCreationParam
            {
                IsLocalTest = true,
                Amount = 100m,
                MinAmount = 100m,
                BalBroughtForward = 1000m,
                AffId = Constant.DefaultAffId,
                Language = Constant.DefaultLanguage,
                TransId = 123,
                TransDate = null
            };
        }

        private static PaymentService CreatePaymentService(
            IAccountRepository accountRepository = null,
            IPaymentRepository paymentRepository = null,
            IEmailService emailService = null,
            IAccountService accountService = null)
        {
            var fakeAccountRepository = accountRepository ?? Mock.Of<IAccountRepository>();
            var fakePaymentRepository = paymentRepository ?? Mock.Of<IPaymentRepository>();
            var fakeEmailService = emailService ?? Mock.Of<IEmailService>();
            var fakeAccountService = accountService ?? Mock.Of<IAccountService>();

            return new PaymentService(fakeAccountRepository, fakePaymentRepository, fakeEmailService, fakeAccountService);
        }
    }

    public static class PaymentServiceTestCase
    {
        public static IEnumerable<object[]> CreateForPaymentCreation()
        {
            yield return new object[]
            {
                new PaymentCreationParam
                {
                    IsLocalTest = true,
                    Amount = 100m,
                    MinAmount = 100m,
                    BalBroughtForward = 90m
                },
                new ResultInfo { IsOk = false, Message = Error.PaymentAmountInvalid }
            };

            yield return new object[]
            {
                new PaymentCreationParam
                {
                    IsLocalTest = true,
                    Amount = 100m,
                    MinAmount = 100m,
                    BalBroughtForward = 1000m,
                    AffId = Constant.DefaultAffId,
                    Language = Constant.DefaultLanguage
                },
                new ResultInfo { IsOk = true }
            };
        }
    }
}
