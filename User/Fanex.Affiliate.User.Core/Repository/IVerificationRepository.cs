using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.VerificationAccount;
using Fanex.Affiliate.User.Core.MetalModels;

namespace Fanex.Affiliate.User.Core.Repository
{
    public interface IVerificationRepository
    {
        ResultInfo UpdateVerificationInfo(AuthResultBaseParam param);

        VerificationInfo GetLoginInfoByUserDevice(int affId, string ipAddress, string deviceInfo);

        ResultInfo InsertUserDeviceInfo(AuthResultBaseParam param);
    }
}
