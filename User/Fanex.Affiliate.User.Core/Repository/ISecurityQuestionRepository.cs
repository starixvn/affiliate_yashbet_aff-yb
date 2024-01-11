using Fanex.Affiliate.User.Core.DTO;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.Repository
{
    public interface ISecurityQuestionRepository
    {
        IEnumerable<SecurityQuestion> GetSecurityQuestions(string language);
    }
}
