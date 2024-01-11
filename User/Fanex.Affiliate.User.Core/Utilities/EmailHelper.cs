using System;

namespace Fanex.Affiliate.User.Core.Utilities
{
    public static class EmailHelper
    {
        private const int MandatoryLengthForSplittingEmail = 2;

        public static string HideEmailPart(string email, int emailLengthToCheck = 8)
        {
            var emailParts = email.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            if (emailParts.Length != MandatoryLengthForSplittingEmail)
            {
                return string.Empty;
            }

            var emailName = emailParts[0];
            var emailSuffix = emailParts[1];

            var newEmailName = emailName.Length > emailLengthToCheck ?
                ReplaceEmailName(emailName, 4, 1) :
                ReplaceEmailName(emailName, 1, 0);

            return newEmailName + "@" + emailSuffix;
        }

        private static string ReplaceEmailName(string emailName, int charLengthToShow, int startIndex)
        {
            var hiddenCharCount = emailName.Length - charLengthToShow;
            var stringReplace = new string('•', hiddenCharCount);
            var convertedEmailName = emailName.Remove(startIndex, hiddenCharCount)
                                                .Insert(startIndex, stringReplace);

            return convertedEmailName;
        }
    }
}
