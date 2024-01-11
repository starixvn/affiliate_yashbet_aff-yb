using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.MetalModels.UserPanel;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface INewsService
    {
        LatestHotNews GetLatestHotNews(bool isInternal);

        IEnumerable<MessageHistory> GetMessageList(MessageHistoryParam param);
    }
}
