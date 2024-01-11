
namespace Fanex.Affiliate.Admin.Core.DTO.Administrator
{
    public class PolicyInfo
    {
        public int ParentId { get; set; }

        public int Policy { get; set; }

        public int FunctionId { get; set; }

        public string FunctionDescription { get; set; }

        public byte GroupPolicy { get; set; }
    }
}
