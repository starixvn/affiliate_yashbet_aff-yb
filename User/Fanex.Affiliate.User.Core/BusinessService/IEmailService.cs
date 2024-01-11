using Fanex.Affiliate.User.Core.MetalModels.Email;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface IEmailService
    {
        void SendRecoverPasswordEmail(RecoverPasswordEmailParam emailParam);

        void SendNewPasswordEmail(NewPasswordEmailParam emailParam);

        void SendPaymentEmail(PaymentEmailParam emailParam);

        void SendNoticeEmailToCsd(NoticeEmailParam emailParam);

        void SendVerificationEmail(VerificationAccountEmailParam emailParam);
    }
}
