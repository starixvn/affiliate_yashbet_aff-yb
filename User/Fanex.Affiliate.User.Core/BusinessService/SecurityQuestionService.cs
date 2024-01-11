using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.Repository;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.BusinessService
{
    public class SecurityQuestionService : ISecurityQuestionService
    {
        private readonly ISecurityQuestionRepository _securityQuestionRepository;

        public SecurityQuestionService(ISecurityQuestionRepository securityQuestionRepository)
        {
            _securityQuestionRepository = securityQuestionRepository;
        }

        public IEnumerable<SecurityQuestion> GetSecurityQuestions(string language)
        {
            return _securityQuestionRepository.GetSecurityQuestions(language);
        }
    }
}
