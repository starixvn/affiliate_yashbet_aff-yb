using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Message;
using Fanex.Affiliate.Admin.Core.MetaModels.Message;

namespace Fanex.Affiliate.Admin.Core.Services.Message
{
    public interface IMessageService
    {
        EntityResult<MessageInfo> GetMessageList(MessageParam param);

        MessageDetail GetMessageDetail(int selectedMessageId);

        ResultInfo InsertMessage(InsertUpdateMessageParam param);

        ResultInfo UpdateMessage(InsertUpdateMessageParam param);

        ResultInfo DeleteMessages(DeleteMessageParam param);
    }
}
