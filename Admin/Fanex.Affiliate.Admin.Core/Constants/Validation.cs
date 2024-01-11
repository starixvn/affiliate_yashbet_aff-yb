namespace Fanex.Affiliate.Admin.Core.Constants
{
    public static class Validation
    {
        public const int MinId = 1;
        public const int MaxId = int.MaxValue;
        public const int MinPageIndex = 1;
        public const int MaxPageIndex = int.MaxValue;
        public const int MinPageSize = 1;
        public const int MaxPageSize = 500;
        public const int MaxSearchBy = int.MaxValue;

        public const long InfinityAmount = 100_000_000_000;
        public const int InfinityQualifiedMember = 1_000_000_000;

        public const int MinAffType = 1;
        public const int MaxAffType = 3;

        public const int MaxLengthCommGroupDescription = 100;
        public const int MaxLengthLandingPage = 100;

        public const int OnlyOneRow = 1;

        public const int Zero = 0;
        public const int MaxPercentage = 1;

        public const int MaxLengthInputId = 9;
        public const int MaxLengthInputString = 30;
    }
}
