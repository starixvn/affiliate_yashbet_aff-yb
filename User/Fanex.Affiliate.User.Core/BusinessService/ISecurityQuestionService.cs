using Fanex.Affiliate.User.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public interface ISecurityQuestionService
    {
        IEnumerable<SecurityQuestion> GetSecurityQuestions(string language);
    }
}
