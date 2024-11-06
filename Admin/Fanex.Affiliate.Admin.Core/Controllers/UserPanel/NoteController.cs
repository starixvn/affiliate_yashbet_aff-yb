using Fanex.Affiliate.Admin.Core.Attributes;
using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO.UserPanel;
using Fanex.Affiliate.Admin.Core.Extensions;
using Fanex.Affiliate.Admin.Core.MetaModels;
using Fanex.Affiliate.Admin.Core.MetaModels.UserPanel;
using Fanex.Affiliate.Admin.Core.Services.Common;
using Fanex.Affiliate.Admin.Core.Services.UserPanel;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Controllers.UserPanel
{
    [FunctionAuthorize(FunctionName = Function.Affiliate)]
    public class NoteController : ReadOnlySessionBaseController
    {
        private readonly INoteService _noteService;

        public NoteController(
            ISessionService sessionService,
            INoteService noteService) : base(sessionService)
        {
            _noteService = noteService;
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult GetNoteList(AffiliatePagingParam param)
        {
            if (ModelState.IsValid)
            {
                return GenerateJsonNetResult(_noteService.GetNoteList(param));
            }

            return new JsonNetResult();
        }

        [HttpPost]
        [ValidateAntiForgeryHeader]
        [FunctionAuthorize(FunctionName = Function.Note)]
        public ActionResult GetNoteContentById(int affId, int noteId)
        {
            if (affId >= Validation.MinId && noteId >= Validation.MinId)
            {
                var noteInfo = _noteService.GetNoteContentById(affId, noteId);
                return GenerateJsonNetResult(noteInfo != null ? new Note { NoteId = noteInfo.NoteId, NoteContent = Server.HtmlDecode(noteInfo.NoteContent) } : null);
            }

            return new JsonNetResult();
        }

        [FunctionAuthorize(FunctionName = Function.Note, IsHost = true)]
        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult InsertNote(InsertUpdateNoteParam param)
        {
            if (ModelState.IsValid)
            {
                var insertNoteParam = AssignCommonParam(param);

                return GenerateJsonNetResult(_noteService.InsertNote(insertNoteParam));
            }

            return GenerateErrResult("Insert Note is failed!");
        }

        [FunctionAuthorize(FunctionName = Function.Note, IsHost = true)]
        [HttpPost]
        [ValidateAntiForgeryHeader]
        public ActionResult UpdateNote(InsertUpdateNoteParam param)
        {
            if (ModelState.IsValid && param.NoteId >= Validation.MinId)
            {
                var updateNoteParam = AssignCommonParam(param);

                return GenerateJsonNetResult(_noteService.UpdateNote(updateNoteParam));
            }

            return GenerateErrResult("Update Note is failed!");
        }

        private InsertUpdateNoteParam AssignCommonParam(InsertUpdateNoteParam param)
        {
            var noteParam = new InsertUpdateNoteParam
            {
                AffId = param.AffId,
                NoteId = param.NoteId,
                NoteContent = Server.HtmlEncode(param.NoteContent)
            };
            AssignUserInfoParam(noteParam);

            return noteParam;
        }
    }
}
