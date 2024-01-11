using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.UserPanel
{
    public class InsertUpdateNoteParam : AffiliateBasicInfoParam
    {
        public int NoteId { get; set; }

        [Required]
        public string NoteContent { get; set; }
    }
}
