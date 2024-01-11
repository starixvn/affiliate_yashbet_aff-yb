namespace Fanex.Affiliate.Link.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// Common class.
    /// </summary>
    public static class Common
    {
        private const int ValidLength = 4;
        private const string IpRegex = "((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\\.|$)){4}";

        /// <summary>
        /// Contains all operating system mapping.
        /// </summary>
        private static IDictionary<string, string> _operatingSystemMapping = new Dictionary<string, string>
        {
                { "Windows NT 6.1", "Windows 7" },
                { "Windows NT 6.0", "Windows Vista" },
                { "Windows NT 5.1", "Windows XP" },
                { "Windows NT 5.0", "Windows 2000" },
                { "Windows NT 4.1", "Windows 98" },
                { "iPhone", "iOS" },
                { "iPad", "iOS" },
                { "iPod", "iOS" },
                { "Mac OS", "Mac OS" },
                { "BlackBerry", "BlackBerry" },
                { "Android", "Android" },
                { "Linux", "Linux" },
                { "Ubuntu", "Ubuntu" }
        };

        /// <summary>
        /// Contains all device mapping.
        /// </summary>
        private static IDictionary<string, string> _deviceMapping = new Dictionary<string, string>
        {
                { "iphone", "iPhone" },
                { "ipad", "iPad" },
                { "ipod", "iPod" },
                { "blackberry", "BlackBerry" },
                { "htc", "HTC" },
                { "motorola", "Motorola" },
                { "nokia", "Nokia" }
        };

        /// <summary>
        /// Get Country.
        /// </summary>
        /// <returns>Country English Name.</returns>
        public static string GetCountry()
        {
            var regionInfo = ResolveCountry();

            return regionInfo != null ? regionInfo.EnglishName : CultureInfo.CurrentCulture.ToString();
        }

        /// <summary>
        /// Get region info.
        /// </summary>
        /// <returns>Region info.</returns>
        private static RegionInfo ResolveCountry()
        {
            var culture = ResolveCulture();

            //solving a culture issue
            if (culture != null && culture.LCID != 0x7F)
            {
                return new RegionInfo(culture.Name);
            }
            return null;
        }

        /// <summary>
        /// Get culture info.
        /// </summary>
        /// <returns>Culture info.</returns>
        private static CultureInfo ResolveCulture()
        {
            var languages = HttpContext.Current.Request.UserLanguages;

            if (languages == null || languages.Length == 0)
            {
                return null;
            }

            try
            {
                var language = languages[0].ToLowerInvariant().Trim();
                return CultureInfo.CreateSpecificCulture(language);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get IP address from client request.
        /// </summary>
        /// <returns>String of IP address.</returns>
        public static string GetIPAddressOfClient()
        {
            string ips = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string ip = GetForwardedIPAddress(ips) ?? HttpContext.Current.Request.UserHostAddress;

            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }

            return ip;
        }

        private static string GetForwardedIPAddress(string ips)
        {
            if (string.IsNullOrEmpty(ips))
            {
                return null;
            }

            var ipList = ips.Split(',');

            foreach (var ip in ipList)
            {
                if (Regex.IsMatch(ip, IpRegex))
                {
                    return ip;
                }
            }

            return null;
        }

        /// <summary>
        /// Get device info.
        /// </summary>
        /// <param name="userAgent">User's info.</param>
        /// <returns>Device info.</returns>
        public static string GetDeviceName(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                return @"Computer/Laptop";
            }

            var deviceKey = _deviceMapping.Keys.FirstOrDefault(key => userAgent.ToLower().Contains(key));

            return string.IsNullOrEmpty(deviceKey) ? (IsMobile() ? "Mobile" : @"Computer/Laptop") : _deviceMapping[deviceKey];
        }

        /// <summary>
        /// Get OS info.
        /// </summary>
        /// <param name="userAgent">User's info.</param>
        /// <returns>Operating system info.</returns>
        public static string GetOsName(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                return string.Empty;
            }

            var osKey = _operatingSystemMapping.Keys.FirstOrDefault(key => userAgent.Contains(key));

            return string.IsNullOrEmpty(osKey) ? string.Empty : _operatingSystemMapping[osKey];
        }

        /// <summary>
        /// Encrypt a string using SHA256.
        /// </summary>
        /// <param name="inputString">Input string.</param>
        /// <returns>Encrypted string.</returns>
        public static string EncryptUsingSHA256(string inputString)
        {
            var hashAlgorithm = new SHA256Managed();
            var hashData = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));

            return BitConverter.ToString(hashData).Replace("-", string.Empty).ToLower();
        }

        public static bool IsMobile()
        {
            var request = HttpContext.Current.Request;
            if (request.Browser.IsMobileDevice)
            {
                return true;
            }

            var u = request.ServerVariables["HTTP_USER_AGENT"];
            if (string.IsNullOrEmpty(u) || u.Length < ValidLength)
            {
                return false;
            }

            var b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return (b.IsMatch(u) || v.IsMatch(u.Substring(0, ValidLength)));
        }

        public static string GetDomainReferrer(string urlReferrer)
        {
            if (Uri.IsWellFormedUriString(urlReferrer, UriKind.Absolute))
            {
                var referrerUri = new Uri(urlReferrer);
                return string.IsNullOrEmpty(referrerUri.Host) ? null : referrerUri.Host;
            }

            return null;
        }
    }
}