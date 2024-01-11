using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.User.Core.MetalModels
{
    public class InternalLoginParam
    {
        [Required]
        public string ChalengeStr { get; set; }

        [Required]
        public string AccKey { get; set; }

        [Required]
        public string CustName { get; set; }

        [Required]
        public int AccId { get; set; }

        [Required]
        public int AccOnId { get; set; }
    }
}
