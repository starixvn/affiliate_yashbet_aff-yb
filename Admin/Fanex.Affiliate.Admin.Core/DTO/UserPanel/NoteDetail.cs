using Newtonsoft.Json;
using System;

namespace Fanex.Affiliate.Admin.Core.DTO.UserPanel
{
    [Serializable]
    public class NoteDetail : Note
    {
        public string CreatedBy { get; set; }

        [JsonIgnore]
        public DateTime? LastModify { get; set; }

        public string LastModifyText => LastModify?.ToString("MM/dd/yyyy HH:mm:ss") ?? "N/A";
    }
}
