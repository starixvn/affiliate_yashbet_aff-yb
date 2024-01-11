using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Message;
using Fanex.Affiliate.Admin.Core.MetaModels.Message;
using Fanex.Affiliate.Admin.Core.Resources;
using Fanex.Affiliate.Admin.Core.Services;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.Message;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers
{
    [FunctionAuthorize(FunctionName = Function.HotNews)]
    public class MessageController : ReadOnlySessionBaseController
    {
        private const int threeMilliSecond = 3;
        private readonly IMessageService _messageSevice;
        private readonly ILanguageService _languageService;

        public MessageController(
            ISessionService sessionService,
            IMessageService messageSevice,
            ILanguageService languageService) : base(sessionService)
        {
            _messageSevice = messageSevice;
            _languageService = languageService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var mode = _languageService.GetLanguageList();

            return View(mode);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetMessageList(MessageParam param)
        {
            if (ModelState.IsValid)
            {
                param.ToDate = param.ToDate.AddDays(1).AddMilliseconds(-threeMilliSecond);

                return GenerateJsonNetResult(_messageSevice.GetMessageList(param));
            }

            return GenerateJsonNetResult(new EntityResult<MessageInfo>
            {
                MainData = new List<MessageInfo>(),
                TotalItem = 0
            });
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetMessageDetail(int selectedMessageId)
        {
            return GenerateJsonNetResult(_messageSevice.GetMessageDetail(selectedMessageId));
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.HotNews, IsHost = true)]
        public ActionResult InsertMessage(InsertUpdateMessageParam param)
        {
            if (param.IsValid())
            {
                var encodeParam = EncodeParam(param);
                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_messageSevice.InsertMessage(encodeParam));
            }

            return GenerateErrResult(Lang.PleaseEnterContent);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.HotNews, IsHost = true)]
        public ActionResult UpdateMessage(InsertUpdateMessageParam param)
        {
            var isValidMessageId = param.MessageId >= Validation.MinId;
            if (param.IsValid() && isValidMessageId)
            {
                var encodeParam = EncodeParam(param);
                AssignUserInfoParam(encodeParam);

                return GenerateJsonNetResult(_messageSevice.UpdateMessage(encodeParam));
            }

            return GenerateErrResult(!isValidMessageId ? Lang.MessageIdIsInvalid : Lang.PleaseEnterContent);
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.HotNews, IsHost = true)]
        public ActionResult DeleteMessages(DeleteMessageParam param)
        {
            if (ModelState.IsValid)
            {
                AssignUserInfoParam(param);

                return GenerateJsonNetResult(_messageSevice.DeleteMessages(param));
            }

            return GenerateErrResult(Lang.PleaseSelectMessage);
        }

        private InsertUpdateMessageParam EncodeParam(InsertUpdateMessageParam param)
        {
            return new InsertUpdateMessageParam
            {
                MessageId = param.MessageId,
                Important = param.Important,
                MessageContenten = Server.HtmlEncode(param.MessageContenten),
                MessageContentcs = Server.HtmlEncode(param.MessageContentcs),
                MessageContentth = Server.HtmlEncode(param.MessageContentth),
                MessageContentvn = Server.HtmlEncode(param.MessageContentvn),
                MessageContentid = Server.HtmlEncode(param.MessageContentid),
                MessageContenthi = Server.HtmlEncode(param.MessageContenthi),
                MessageContentms = Server.HtmlEncode(param.MessageContentms),
                MessageContentbn = Server.HtmlEncode(param.MessageContentbn),
                MessageContentkh = Server.HtmlEncode(param.MessageContentkh),
            };
        }
    }
}
