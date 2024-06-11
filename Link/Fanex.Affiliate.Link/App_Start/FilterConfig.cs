namespace Fanex.Affiliate.Link
{
    using System.Web.Mvc;

    /// <summary>
    /// Filter configuration.
    /// </summary>
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}