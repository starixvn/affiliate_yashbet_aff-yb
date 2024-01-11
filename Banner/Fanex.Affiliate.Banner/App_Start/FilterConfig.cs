namespace Affiliate.Banner
{
    using System.Web.Mvc;

    /// <summary>
    /// Filter Config.
    /// </summary>
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}