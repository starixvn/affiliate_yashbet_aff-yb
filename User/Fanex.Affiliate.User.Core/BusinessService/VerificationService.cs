using Fanex.Affiliate.User.Core.Constants;
using Fanex.Affiliate.User.Core.DTO.VerificationAccount;
using Fanex.Affiliate.User.Core.Extensions;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.MetalModels.Email;
using Fanex.Affiliate.User.Core.Repository;
using Fanex.Affiliate.User.Core.Repository.UserPanel;
using Fanex.Affiliate.User.Core.Utilities;
using OtpNet;
using System.Security.Cryptography;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class VerificationService : IVerificationService
    {
        private const int SecretKeyExistedCode = 101;

        private readonly ITwoFactorAuthService _twoFactorAuthService;
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailService _mailService;
        private readonly ICryptographyService _cryptographyService;
        private readonly IWrapperUtility _wrapperUtility;
        private readonly IVerificationRepository _verificationRepository;

        public VerificationService(
            ITwoFactorAuthService twoFactorAuthService,
            IAccountRepository accountRepository,
            IEmailService mailService,
            ICryptographyService cryptographyService,
            IWrapperUtility wrapperUtility,
            IVerificationRepository verificationRepository)
        {
            _twoFactorAuthService = twoFactorAuthService;
            _accountRepository = accountRepository;
            _mailService = mailService;
            _cryptographyService = cryptographyService;
            _wrapperUtility = wrapperUtility;
            _verificationRepository = verificationRepository;
        }

        public SendCodeResult SendCode(SendCodeParam param)
        {
            if (CanSendCode(param.SentCodeTime, param.IntervalInMinute))
            {
                var userInfo = _accountRepository.GetUserInfoById(param.AffId, false);
                if (!param.Email.MatchWith(userInfo.Email))
                {
                    return CreateSendCodeFailedResult(isValidEmail: false);
                }

                if (string.IsNullOrEmpty(userInfo.SecretKey))
                {
                    userInfo.SecretKey = TryCreateSecretKey(userInfo.AffId);
                }

                var counter = _wrapperUtility.GetRandomGenerator().Next(int.MinValue, int.MaxValue);
                var otp = _twoFactorAuthService.GenerateHotp(userInfo.SecretKey, counter);

                var emailParam = param.VerificationEmailParam;
                var verificationAccountEmailParam = new VerificationAccountEmailParam
                {
                    User = userInfo,
                    MailContact = emailParam.MailContact,
                    MailTo = userInfo.Email,
                    RootDomain = emailParam.RootDomain,
                    Code = otp,
                    Ip = emailParam.Ip
                };

                _mailService.SendVerificationEmail(verificationAccountEmailParam);

                return new SendCodeResult
                {
                    IsValidEmail = true,
                    CanSendCode = true,
                    NewCounter = counter,
                    NewSendCodeTime = GetNewSendCodeTime()
                };
            }

            return CreateSendCodeFailedResult(false);
        }

        public bool IsValidOtp(VerificationOtpParam param)
        {
            var authMessage = param.LoginInfo + param.HashKey + param.Captcha + param.OtpCode;
            var hashMessage = CreateHashMessage(authMessage, param.HashKey);

            var isValid = string.Equals(hashMessage, param.HashMessage) &&
                _twoFactorAuthService.VerifyHotp(param.SecretKey, param.Counter, param.OtpCode);

            UpdateVerificationInfo(param, isValid);

            return isValid;
        }

        public VerificationAction CreateActionLink(string key, string salt, int expirationInMinute)
        {
            var nowTicks = _wrapperUtility.GetNowDateTime().Ticks.ToString();
            var queryString = "controller=VerificationAccount&action=VerifyAccount&t=" + nowTicks + "&e=" + expirationInMinute;

            return new VerificationAction
            {
                Link = ConverterUtils.Base64Encode(SymmetricEncryptionUtility.Encrypt<AesManaged>(queryString, key, salt)),
                TimeToken = nowTicks
            };
        }

        private void UpdateVerificationInfo(VerificationOtpParam param, bool isValid)
        {
            var verificationParam = CreateVerificationParam(param);
            var verificationInfo = _verificationRepository.GetLoginInfoByUserDevice(
                    verificationParam.AffId,
                    verificationParam.IpAddress,
                    verificationParam.DeviceInfo);

            if (verificationInfo.FailedLoginCount.HasValue)
            {
                var updateResult = _verificationRepository.UpdateVerificationInfo(verificationParam);
                if (!updateResult.IsOk)
                {
                    LogHelper.WriteError(updateResult.Message);
                }
            }
            else
            {
                if (isValid)
                {
                    var insertResult = _verificationRepository.InsertUserDeviceInfo(verificationParam);
                    if (!insertResult.IsOk)
                    {
                        LogHelper.WriteError(insertResult.Message);
                    }
                }
            }
        }

        private AuthResultBaseParam CreateVerificationParam(VerificationOtpParam param)
        {
            return new AuthResultBaseParam
            {
                AffId = param.AffId,
                DeviceInfo = _cryptographyService.ComputeSha256Hash(Parser.GetDeviceInfo(param.UserAgent)),
                IpAddress = param.IpAddress,
                Language = param.Language
            };
        }

        private bool CanSendCode(long sendCodeTimeSession, int intervalInMinute)
        {
            var maxTime = _wrapperUtility.GetNowDateTime().AddMinutes(intervalInMinute * -1).Ticks;

            return sendCodeTimeSession == 0 || maxTime - sendCodeTimeSession >= 0;
        }

        private long GetNewSendCodeTime()
        {
            var now = _wrapperUtility.GetNowDateTime();

            return DateTimeHelper.CreateDateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second).Ticks;
        }

        private static string CreateHashMessage(string message, string secret)
            => CryptographyHelper.SHA256Hash(message, secret);

        private string TryCreateSecretKey(int affId)
        {
            var secretKey = GenerateSecretKey();
            var resultCode = _accountRepository.CreateSecretKey(affId, secretKey);

            while (resultCode == SecretKeyExistedCode)
            {
                secretKey = GenerateSecretKey();
                resultCode = _accountRepository.CreateSecretKey(affId, secretKey);
            }

            return secretKey;
        }

        private static string GenerateSecretKey()
        {
            return Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(Number.Twenty));
        }

        private static SendCodeResult CreateSendCodeFailedResult(bool canSendCode = true, bool isValidEmail = true)
        {
            return new SendCodeResult
            {
                IsValidEmail = isValidEmail,
                CanSendCode = canSendCode
            };
        }
    }
}