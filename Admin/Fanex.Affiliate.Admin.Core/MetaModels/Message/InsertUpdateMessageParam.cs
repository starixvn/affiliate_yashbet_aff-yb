using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.Admin.Core.MetaModels.Message
{
    public class InsertUpdateMessageParam : UserInfoParam
    {
        public int MessageId { get; set; }

        public bool Important { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MessageContenten { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MessageContentcs { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MessageContentth { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MessageContentvn { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MessageContentid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MessageContenthi { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MessageContentms { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MessageContentbn { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MessageContentkh { get; set; }

        public bool IsValid()
        {
            if (!(string.IsNullOrEmpty(MessageContenten) && string.IsNullOrEmpty(MessageContentcs) && string.IsNullOrEmpty(MessageContentbn)))
            {
                return true;
            }

            if (!(string.IsNullOrEmpty(MessageContentth) && string.IsNullOrEmpty(MessageContentvn) && string.IsNullOrEmpty(MessageContentid)))
            {
                return true;
            }

            return !(string.IsNullOrEmpty(MessageContenthi) && string.IsNullOrEmpty(MessageContentms) && string.IsNullOrEmpty(MessageContentkh));
        }
    }
}
