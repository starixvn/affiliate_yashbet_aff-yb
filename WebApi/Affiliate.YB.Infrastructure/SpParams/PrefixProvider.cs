namespace Affiliate.YB.Core
{
    public interface IPrefixProvider
    {
        string PrefixCode { get; }
    }

    public class PrefixProvider : IPrefixProvider
    {
        public PrefixProvider(string prefixCode)
        {
            PrefixCode = prefixCode;
        }

        public string PrefixCode { get; }
    }
}