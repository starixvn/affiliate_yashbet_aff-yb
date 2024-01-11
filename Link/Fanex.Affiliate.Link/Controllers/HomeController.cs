using System.Configuration;

namespace Fanex.Affiliate.Link.Controllers
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Text;
    using System.Web.Mvc;
    using Commands;
    using Fanex.Logging;
    using Helpers;
    using ONEbook.UM.AspNetMVC.Filters;
    using Services.Impl;
    using Services.Interface;

    /// <summary>
    /// Home controller for affiliate link.
    /// </summary>
    [CheckUMFilter]
    public class HomeController : Controller
    {
        #region Services

        private readonly ITrackingSystemService _trackingSystemService;
        private readonly IGeneralAffiliateInfoService _generalAffiliateInfoService;

        private const string LinkIsInvalidMessage = "This link does not exists. Please contact administrator!";
        private const string LinkIsIncorrectMessage = "This link is incorrect. Please contact administrator!";

        #endregion Services

        #region Constructor

        public HomeController()
        {
            _trackingSystemService = new TrackingSystemService();
            _generalAffiliateInfoService = new GeneralAffiliateInfoService();
        }

        #endregion Constructor

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PageNotFound()
        {
            return View();
        }

        public ActionResult Link(string affId, string pageId, string lang = null, string deviceType = null)
        {
            try
            {
                var affInfo = _generalAffiliateInfoService.GetGeneralAffiliateInfo(affId: Convert.ToInt32(affId));
                var affDomain = GetFullAffDomain(GetOriginalUrl(), affId.ToString());

                return InsertAffiliate(affInfo.affId, affInfo.affCode, pageId, lang, affDomain);
            }
            catch(Exception ex)
            {
                LogService.Error($"{ex.Message}", ex);
                ViewBag.Result = LinkIsIncorrectMessage;
                return View("Link");
            }
        }

        public ActionResult NewLink(string pageId, string affCode = null, string lang = null)
        {
            try
            {
                var affInfo = _generalAffiliateInfoService.GetGeneralAffiliateInfo(affCode: affCode);
                var affDomain = GetFullAffDomain(GetOriginalUrl(), pageId);

                return InsertAffiliate(affInfo.affId, affInfo.affCode, pageId, lang, affDomain);
            }
            catch (Exception ex)
            {
                LogService.Error($"{ex.Message}", ex);
                ViewBag.Result = LinkIsIncorrectMessage;
                return View("Link");
            }
        }

        private ActionResult InsertAffiliate(int affId, string affCode, string pageId, string lang, string affDomain)
        {
            if (Request.Url != null)
            {
                var urlReferrer = Request.UrlReferrer?.AbsoluteUri;
                var domainReferrer = Request.UrlReferrer?.Host;

                if (string.IsNullOrEmpty(urlReferrer) && !string.IsNullOrEmpty(Request["ref"]))
                {
                    urlReferrer = Request["ref"];
                    domainReferrer = Common.GetDomainReferrer(urlReferrer);
                }

                LogService.Info(GetAllHeader(Request.ServerVariables));

                var command = new TrackingSystemCommand
                {
                    AffDomain = affDomain,
                    AffId = affId,
                    PageCode = pageId,
                    LanguageCode = lang,
                    DeviceType = GetDeviceType(),
                    IP = Common.GetIPAddressOfClient(),
                    Browser = Request.Browser.Browser,
                    URLReferrer = urlReferrer,
                    DomainReferrer = domainReferrer,
                    OS = Common.GetOsName(Request.UserAgent),
                    Device = Common.GetDeviceName(Request.UserAgent),
                    BrowserVersion = Request.Browser.Version,
                    BrowserResolution = Request.Browser.ScreenPixelsWidth + "x" + Request.Browser.ScreenPixelsHeight,
                    BrowserLanguage = string.IsNullOrEmpty(Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"])
                                              ? string.Empty
                                              : Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"],
                    UserAgent = Request.UserAgent,
                    Country = Common.GetCountry(),
                    AffMediaLink = GetOriginalUrl()
                };

                var redirectUrl = _trackingSystemService.InsertStatistic(command);

                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    var result = HandleRedirectUrl(redirectUrl, pageId, command.LanguageCode, affCode);

                    return Redirect(result);
                }

                ViewBag.Result = LinkIsInvalidMessage;
            }

            return View("Link");
        }

        private string GetFullAffDomain(string url, string endPointOfAffDomain)
        {
            if (byte.Parse(ConfigurationManager.AppSettings["RemovePortFromUrl"]) == 1)
            {
                var uri = new Uri(url);
                url = url.Replace(":" + uri.Port, string.Empty);
            }

            const string slashString = "://";
            var pos1 = url.IndexOf(slashString, 0, StringComparison.OrdinalIgnoreCase);
            var pos2 = url.IndexOf("/" + endPointOfAffDomain, 0, StringComparison.OrdinalIgnoreCase);

            return url.Substring(pos1 + slashString.Length, pos2 - (pos1 + slashString.Length));
        }

        private string GetOriginalUrl()
        {
            var host = Request.Headers["Host"] ?? Request.Url.Host;
            var scheme = GetOriginalProtocol();
            var path = Request.Url.AbsolutePath;
            return $"{scheme}://{host}{path}";
        }

        private byte GetDeviceType()
        {
            return (byte)(Common.IsMobile() ? 1 : 0);
        }

        private string HandleRedirectUrl(string redirectUrl, string pageId, string languageCode, string affCode)
        {
            var redirectPattern = ConfigurationManager.AppSettings[$"Redirect_{pageId.ToLowerInvariant()}_pattern"];
            if (!string.IsNullOrEmpty(redirectPattern))
            {
                var redirectUri = new Uri(redirectUrl);
                var pathAndQueryString = redirectPattern.Replace("#language#", languageCode).Replace("#affCode#", affCode);
                return $"{redirectUri.Scheme}://{redirectUri.Host}{pathAndQueryString}";
            }
            else
            {
                var url = redirectUrl.Split('?');
                return $"{url[0]}?affCode={affCode}";
            }
        }

        private string GetAllHeader(NameValueCollection headers)
        {
            var builder = new StringBuilder();
            foreach (var key in headers.AllKeys)
            {
                builder.AppendLine($"{key}: {headers[key]}");
            }

            return builder.ToString();
        }

        private string GetOriginalProtocol()
        {
            var refLink = Request["Referer"];
            var url = string.IsNullOrWhiteSpace(refLink) ? Request.Url : new Uri(refLink);
            var protos = Request.ServerVariables["HTTP_X_FORWARDED_PROTO"]?.Split(',');
            var origProto = protos != null && protos.Length > 0 ? protos[0].Trim() : url.Scheme;
            return origProto;
        }
    }
}