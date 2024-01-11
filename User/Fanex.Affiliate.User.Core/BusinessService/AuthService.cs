using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.DTO.VerificationAccount;
using Fanex.Affiliate.User.Core.MetalModels;
using Fanex.Affiliate.User.Core.Repository;
using Fanex.Affiliate.User.Core.Resources;
using Fanex.Affiliate.User.Core.Utilities;
using Fanex.Affiliate.User.Core.ViewModels;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class AuthService : IAuthService
    {
        private const int MaxFailedLoginCount = 5;
        private const int AllowedUserDeviceCount = 10;

        private readonly IAuthRepository _authRepository;
        private readonly IVerificationRepository _verificationRepository;
        private readonly ICryptographyService _cryptographyService;
        private readonly IAppSetting _appSetting;

        public AuthService(
            IAuthRepository authRepository,
            ICryptographyService cryptographyService,
            IAppSetting appSetting,
            IVerificationRepository verificationRepository)
        {
            _authRepository = authRepository;
            _cryptographyService = cryptographyService;
            _appSetting = appSetting;
            _verificationRepository = verificationRepository;
        }

        public LoginResultViewModel GetLoginResult(UserLoginParam userParam, bool isInternal)
        {
            return isInternal ? GetLoginResultForInternalAdmin(userParam) : GetLoginResultForAffUser(userParam);
        }

        public int GetOnUserId(OnUserIdParam onUserIdParam)
        {
            return _authRepository.GetOnUserId(onUserIdParam);
        }

        public string GetSessionId(int onUserId)
        {
            return _authRepository.GetSessionId(onUserId);
        }

        public bool Recheckin(int onUserId, bool isInternal)
        {
            return _authRepository.Recheckin(onUserId, isInternal);
        }

        public void SignIn(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        private LoginResultViewModel GetLoginResultForInternalAdmin(UserLoginParam userParam)
        {
            var user = _authRepository.GetUserInfo(userParam.UserName, true);
            var checkingActiveUserResult = CheckActiveUser(user, true);

            return new LoginResultViewModel
            {
                UserInfo = user,
                Result = checkingActiveUserResult
            };
        }

        private LoginResultViewModel GetLoginResultForAffUser(UserLoginParam userParam)
        {
            var loginInfoQueryString = _cryptographyService.RsaDecrypt(_appSetting.RsaPrivateKey, userParam.LoginInfo);
            var loginInfo = HttpUtility.ParseQueryString(loginInfoQueryString);
            var queryStringValidationResult = IsValidQueryStringInfo(loginInfo);
            if (!queryStringValidationResult.Result.IsOk)
            {
                return queryStringValidationResult;
            }

            userParam.UserName = loginInfo["u"];
            var user = _authRepository.GetUserInfo(userParam.UserName, false);
            var checkingActiveUserResult = CheckActiveUser(user);
            if (!checkingActiveUserResult.IsOk)
            {
                return new LoginResultViewModel { Result = checkingActiveUserResult };
            }

            var hashedDeviceInfo = _cryptographyService.ComputeSha256Hash(Parser.GetDeviceInfo(userParam.UserAgent));
            if (!userParam.IsVerifyingAccount)
            {
                var verificationInfo = _verificationRepository.GetLoginInfoByUserDevice(user.AffId, userParam.LoginIp, hashedDeviceInfo);
                if (verificationInfo == null || ShouldVerifyAccount(verificationInfo))
                {
                    return new LoginResultViewModel
                    {
                        UserInfo = user,
                        Result = new LoginResultInfo { IsOk = false, ShouldVerifyAccount = true }
                    };
                }
            }

            userParam.Password = loginInfo["p"];
            var isMatchPassword = IsMatchPassword(userParam.Password, user.Password);
            var loginResultParam = CreateLoginResultParam(userParam, isMatchPassword, user.AffId, hashedDeviceInfo);
            var loginResultInfo = _authRepository.UpdateLoginResult(loginResultParam);

            var isSuccessful = loginResultInfo.IsOk && isMatchPassword;

            return new LoginResultViewModel
            {
                UserInfo = user,
                Result = new LoginResultInfo { IsOk = isSuccessful, Message = isSuccessful ? string.Empty : Error.LoginWrongInfo }
            };
        }

        private static LoginResultInfo CheckActiveUser(AuthorizedUserInfo user, bool isInternal = false)
        {
            const string Approved = "Approved";
            const string Suspended = "Suspended";
            if (user == null)
            {
                return new LoginResultInfo { IsOk = false, Message = Error.LoginWrongInfo };
            }

            if (!string.Equals(Approved, user.Status, StringComparison.CurrentCultureIgnoreCase))
            {
                if (isInternal && string.Equals(Suspended, user.Status, StringComparison.CurrentCultureIgnoreCase))
                {
                    return new LoginResultInfo { IsOk = true };
                }

                return new LoginResultInfo { IsOk = false, Message = Error.YourAccountHasBeenSuspended };
            }

            return new LoginResultInfo { IsOk = true };
        }

        private static bool ShouldVerifyAccount(VerificationInfo verificationInfo)
        {
            var failedLoginCount = verificationInfo.FailedLoginCount;

            return (failedLoginCount.HasValue && failedLoginCount.Value >= MaxFailedLoginCount) ||
                (!failedLoginCount.HasValue && verificationInfo.DeviceCount > AllowedUserDeviceCount);
        }

        private LoginResultViewModel IsValidQueryStringInfo(NameValueCollection loginInfo)
        {
            var timeStampString = loginInfo["t"];
            if (!IsValidTimeStamp(timeStampString, _appSetting.LoginTimeout))
            {
                return new LoginResultViewModel
                {
                    Result = new LoginResultInfo { IsOk = false, Message = Error.YourSessionHasExpired }
                };
            }

            var userName = loginInfo["u"];
            var password = loginInfo["p"];
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return new LoginResultViewModel
                {
                    Result = new LoginResultInfo
                    {
                        IsOk = false,
                        Message = string.IsNullOrEmpty(userName) ? Lang.ErrmsgEntryUsername : Lang.ErrmsgEntryPassword
                    }
                };
            }

            return new LoginResultViewModel
            {
                Result = new LoginResultInfo { IsOk = true }
            };
        }

        private bool IsMatchPassword(string passwordInput, string passwordFromDb)
        {
            string encryptedPassword = _cryptographyService.EncryptPassword(passwordInput);

            return encryptedPassword.Equals(passwordFromDb);
        }

        private LoginResultParam CreateLoginResultParam(UserLoginParam userParam, bool isSuccessful, int affId, string deviceInfo = null)
        {
            return new LoginResultParam
            {
                AffId = affId,
                IpAddress = userParam.LoginIp,
                Language = userParam.Language,
                IsSuccessful = isSuccessful,
                DeviceInfo = deviceInfo
            };
        }

        private static bool IsValidTimeStamp(string timeStampString, int timeout)
        {
            var timestamp = Convert.ToInt64(timeStampString);
            var utcDateTimeRequest = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
            var utcNow = DateTimeHelper.NowDateTime.ToUniversalTime();

            return DateTime.Compare(utcDateTimeRequest, utcNow.AddMinutes(timeout * -1)) >= 0;
        }
    }
}