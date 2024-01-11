using Fanex.Affiliate.User.Core.Resources;

namespace Fanex.Affiliate.User.Core.Utilities
{
    public static class Common
    {
        public static string GetErrorMessage(string functionName, int errorCode)
        {
            var resourceKey = functionName + errorCode.ToString();

            return Error.ResourceManager.GetString(resourceKey);
        }
    }
}
