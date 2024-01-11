using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.Repository;
using NSubstitute;
using System.Linq;
using Xunit;

namespace Fanex.Affiliate.User.Core.UnitTests.Repository
{
    public class GetSecurityQuestionsTest : RepositoryTestBase<SecurityQuestionRepository>
    {
        [Fact]
        public void GetSecurityQuestions_WhenCall_ReturnListSecurityQuestion()
        {
            var repo = CreateRepo();
            var secQuestion = new SecurityQuestion
            {
                Question = "abc",
                QuestionId = 1,
            };
            var spKey = "Aff_User_SecurityQuestions_Sel";
            var db = Builder.Build(null, null);
            var reader = db.ExecuteReader();
            AssignObjectToReader(reader, secQuestion);
            reader["QuestionID"].Returns(secQuestion.QuestionId);
            reader.Read().Returns(true, true, false);

            var result = repo.GetSecurityQuestions("vi-VN");

            Assert.Equal(spKey, db.SettingKey);
            Assert.Equal(2, result.Count());
            reader.Received(3).Read();
        }
    }
}
