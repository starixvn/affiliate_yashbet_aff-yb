using Fanex.Affiliate.User.Core.BusinessService;
using Fanex.Affiliate.User.Core.Enum;
using Fanex.Affiliate.User.Core.MetalModels.Email;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.UnitTests.TestHelper;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels;
using Newtonsoft.Json;
using NSubstitute;
using System;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.BusinessService
{
    public class EmailServiceTest
    {
        private const int AppMailId = 4;

        private readonly IAppSetting AppSetting;
        private readonly IMailProcessor MailProcessor;
        private readonly ILanguageService LangService;

        public EmailServiceTest()
        {
            MailProcessor = Substitute.For<IMailProcessor>();
            AppSetting = Substitute.For<IAppSetting>();
            AppSetting.AppMailId.Returns(AppMailId.ToString());
            LangService = Substitute.For<ILanguageService>();
        }

        [Fact]
        public void SendCodeReoverPasswordEmail_WhenCall_SendMail()
        {
            var service = CreateService();
            var param = CreateRecoverEmailParam();
            var mainContent = "mail content";
            MockingHelper.SetUpEnviroment();
            MailProcessor.BuildTemplateContent(null, null, null).ReturnsForAnyArgs(mainContent);

            service.SendRecoverPasswordEmail(param);

            MailProcessor.Received().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<RecoverPasswordEmailParam>());
            MailProcessor.Received().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<EmailTemplateViewModel>());
            MailProcessor.Received().Send(mainContent, param.MailTo, Lang.SubjectForgetPass);
        }

        [Fact]
        public void SendCodeReoverPasswordEmail_WhenCallWithTextContent_SendMail()
        {
            var service = CreateService();
            var param = CreateRecoverEmailParam();
            param.User.MailOption = true;

            var mainContent = "mail content";
            MockingHelper.SetUpEnviroment();
            MailProcessor.BuildTemplateContent(null, null, null).ReturnsForAnyArgs(mainContent);
            LangService.IsValid(Arg.Any<string>()).Returns(true);

            service.SendRecoverPasswordEmail(param);

            MailProcessor.Received().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<RecoverPasswordEmailParam>());
            MailProcessor.DidNotReceive().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<EmailTemplateViewModel>());
            MailProcessor.Received().Send(mainContent, param.MailTo, Lang.SubjectForgetPass);
        }

        [Fact]
        public void SendNewPasswordEmail_WhenCall_SendMail()
        {
            var service = CreateService();
            var param = CreateNewPasswordEmailParam();
            var mainContent = "mail content";
            MockingHelper.SetUpEnviroment();
            MailProcessor.BuildTemplateContent(null, null, null).ReturnsForAnyArgs(mainContent);

            service.SendNewPasswordEmail(param);

            MailProcessor.Received().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<NewPasswordEmailParam>());
            MailProcessor.Received().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<EmailTemplateViewModel>());
            MailProcessor.Received().Send(mainContent, param.MailTo, Lang.SubjectNewPass);
        }

        [Fact]
        public void SendNewPasswordEmail_WhenCallWithTextContent_SendMail()
        {
            var service = CreateService();
            var param = CreateNewPasswordEmailParam();
            param.User.MailOption = true;

            var mainContent = "mail content";
            MockingHelper.SetUpEnviroment();
            MailProcessor.BuildTemplateContent(null, null, null).ReturnsForAnyArgs(mainContent);

            service.SendNewPasswordEmail(param);

            MailProcessor.Received().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<NewPasswordEmailParam>());
            MailProcessor.DidNotReceive().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<EmailTemplateViewModel>());
            MailProcessor.Received().Send(mainContent, param.MailTo, Lang.SubjectNewPass);
        }

        [Theory]
        [InlineData(PaymentRequestType.Cancel)]
        [InlineData(PaymentRequestType.Create)]
        public void SendPaymentEmail_WhenCall_SendMail(PaymentRequestType type)
        {
            var service = CreateService();
            var param = CreatePaymentEmailParam(type);
            var mainContent = "mail content";
            MockingHelper.SetUpEnviroment();
            MailProcessor.BuildTemplateContent(null, null, null).ReturnsForAnyArgs(mainContent);

            service.SendPaymentEmail(param);

            MailProcessor.Received().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<PaymentEmailParam>());
            MailProcessor.Received().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<EmailTemplateViewModel>());
            MailProcessor.Received().Send(mainContent, param.MailTo, Arg.Compat.Is<string>(c => c.Contains(param.TransId.ToString())));
        }

        [Theory]
        [InlineData(PaymentRequestType.Cancel)]
        [InlineData(PaymentRequestType.Create)]
        public void SendPaymentEmail_WhenCallWithTextContent_SendMail(PaymentRequestType type)
        {
            var service = CreateService();
            var param = CreatePaymentEmailParam(type);
            param.User.MailOption = true;

            var mainContent = "mail content";
            MockingHelper.SetUpEnviroment();
            MailProcessor.BuildTemplateContent(null, null, null).ReturnsForAnyArgs(mainContent);

            service.SendPaymentEmail(param);

            MailProcessor.Received().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<PaymentEmailParam>());
            MailProcessor.DidNotReceive().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<EmailTemplateViewModel>());
            MailProcessor.Received().Send(mainContent, param.MailTo, Arg.Compat.Is<string>(c => c.Contains(param.TransId.ToString())));
        }

        [Fact]
        public void SendNoticeEmailToCsd_WhenCall_SendMail()
        {
            var service = CreateService();
            var param = CreateNoticeEmailParam();
            var mainContent = "mail content";
            MockingHelper.SetUpEnviroment();
            MailProcessor.BuildTemplateContent(null, null, null).ReturnsForAnyArgs(mainContent);

            service.SendNoticeEmailToCsd(param);

            MailProcessor.Received().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<NoticeEmailParam>());
            MailProcessor.Received().Send(mainContent, param.MailTo, Arg.Compat.Is<string>(c => c.Contains(param.UserName)));
        }

        [Fact]
        public void SendVerificationEmail_WhenCall_SendMail()
        {
            var service = CreateService();
            var param = CreateVerificationAccountEmailParam();
            var mainContent = "mail content";
            MockingHelper.SetUpEnviroment();
            MailProcessor.BuildTemplateContent(null, null, null).ReturnsForAnyArgs(mainContent);

            service.SendVerificationEmail(param);

            MailProcessor.Received().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<VerificationAccountEmailParam>());
            MailProcessor.Received().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<EmailTemplateViewModel>());
            MailProcessor.Received().Send(mainContent, param.MailTo, Lang.SubjectVerificationAccount);
        }

        [Fact]
        public void SendVerificationEmail_WhenCallWithTextContent_SendMail()
        {
            var service = CreateService();
            var param = CreateVerificationAccountEmailParam();
            param.User.MailOption = true;

            var mainContent = "mail content";
            MockingHelper.SetUpEnviroment();
            MailProcessor.BuildTemplateContent(null, null, null).ReturnsForAnyArgs(mainContent);

            service.SendVerificationEmail(param);

            MailProcessor.Received().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<VerificationAccountEmailParam>());
            MailProcessor.DidNotReceive().BuildTemplateContent(Arg.Compat.Any<string>(), Arg.Compat.Any<string>(), Arg.Compat.Any<EmailTemplateViewModel>());
            MailProcessor.Received().Send(mainContent, param.MailTo, Lang.SubjectVerificationAccount);
        }

        private static RecoverPasswordEmailParam CreateRecoverEmailParam()
        {
            var serialBaseClass = JsonConvert.SerializeObject(CreateEmailBaseParam());
            var param = JsonConvert.DeserializeObject<RecoverPasswordEmailParam>(serialBaseClass);
            param.Code = "123";
            return param;
        }

        private static NewPasswordEmailParam CreateNewPasswordEmailParam()
        {
            var serialBaseClass = JsonConvert.SerializeObject(CreateEmailBaseParam());
            var param = JsonConvert.DeserializeObject<NewPasswordEmailParam>(serialBaseClass);
            param.NewPassword = "123";
            return param;
        }

        private static PaymentEmailParam CreatePaymentEmailParam(PaymentRequestType type)
        {
            var serialBaseClass = JsonConvert.SerializeObject(CreateEmailBaseParam());
            var param = JsonConvert.DeserializeObject<PaymentEmailParam>(serialBaseClass);
            param.PaymentType = type;
            param.RequestDate = DateTime.Now;
            param.TransId = 1123;
            param.Amount = 20000;
            return param;
        }

        private static NoticeEmailParam CreateNoticeEmailParam()
        {
            var serialBaseClass = JsonConvert.SerializeObject(CreateEmailBaseParam());
            var param = JsonConvert.DeserializeObject<NoticeEmailParam>(serialBaseClass);
            param.FirstName = "first";
            param.LastName = "last";
            param.UserName = "UserName";
            param.SiteUrl = "url";
            param.MoreSiteUrls = "moreurl";
            param.NoSiteDesc = "1";
            return param;
        }

        private static VerificationAccountEmailParam CreateVerificationAccountEmailParam()
        {
            var serialBaseClass = JsonConvert.SerializeObject(CreateEmailBaseParam());
            var param = JsonConvert.DeserializeObject<VerificationAccountEmailParam>(serialBaseClass);
            param.Code = "123";
            param.Ip = "127.0.0.1";
            return param;
        }

        private static EmailBaseParam CreateEmailBaseParam()
            => new EmailBaseParam
            {
                MailContact = "ab",
                MailTo = "12@gmail.com",
                RootDomain = "abc.com",
                User = new DTO.UserInfo
                {
                    AffId = 1,
                    Email = "abb@gmail.com",
                    FirstName = "User",
                    LastName = "Name",
                    MailOption = false,
                    PreferredLanguage = "vi-VN",
                    SecretKey = "124m",
                    UserName = "test"
                }
            };

        private EmailService CreateService()
            => new EmailService(MailProcessor, LangService, AppSetting);
    }
}
