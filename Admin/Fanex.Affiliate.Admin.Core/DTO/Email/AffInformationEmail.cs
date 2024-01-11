namespace Fanex.Affiliate.Admin.Core.DTO.Email
{
    public class AffInformationEmail : EmailBase
    {
        public string AffCode { get; set; }

        public string FirstName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
