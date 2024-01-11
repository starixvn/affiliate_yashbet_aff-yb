using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Customer;
using Fanex.Affiliate.Admin.Core.MetaModels.Customer;
using System.Collections.Generic;

namespace Fanex.Affiliate.Admin.Core.Repository.Customer
{
    public interface ICustomerLeftPanelRepository
    {
        AffiliateInfoOnLeftPanel GetAffiliateInfoOnLeftPanel(string affUserName);

        IEnumerable<OnlineList> GetOnlineList(string affUserName);

        ResultInfo UpdateLanguage(UpdateLanguageParam param);

        ResultInfo UpdateAffType(UpdateAffTypeParam param);

        ResultInfo UpdateStatus(UpdateStatusParam param);

        ResultInfo UpdateClosedStatus(UpdateClosedStatusParam param);

        TransferLimitSettingInfo GetTransferLimitSetting(int affId);

        ResultInfo UpdateTransferLimitSetting(TransferLimitSettingParam param);
    }
}
