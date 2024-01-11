using Fanex.Affiliate.User.Core.DTO;
using Fanex.Affiliate.User.Core.DTO.Auth;
using Fanex.Affiliate.User.Core.DTO.UserPanel;
using Fanex.Affiliate.User.Core.DTO.UserPanel.MediaGallery;
using System.Collections.Generic;

namespace Fanex.Affiliate.User.Core.UnitTests.TestHelper
{
    public static class Constant
    {
        public const string DefaultLanguage = "en-US";

        public const int DefaultAffId = 123;

        public const string AffTypeAsian = "2";

        public const string AffTypeEuro = "4";

        public const string DefaultUserName = "AffB2CTest";

        public const string DefaultMailContact = "abc@yahoo.com";

        public const string DefaultMailCc = "xyz@yahoo.com";

        public const string DefaultMailUser = "user@yahoo.com";

        public const string DefaultRootDomain = "http://abcxyz.com";

        public const string Captcha = "1234";

        public const string Browser = "Chrome";

        public static IEnumerable<Language> LanguageList = new List<Language>
        {
            new Language {Code = "en-UK", DisplayName="English", EnglishName="English", NativeName="English", ShortedCode="en" },
            new Language {Code ="vi-VN", DisplayName="VietNam", EnglishName= "VietNamese", NativeName="Tieng viet", ShortedCode = "vn" },
            new Language {Code = "en-US",  DisplayName="English", EnglishName="English", NativeName="English", ShortedCode="en"}
        };

        public static IEnumerable<LanguageType> LanguageTypeList = new List<LanguageType>
        {
            new LanguageType {DisplayName = "English", MarketId = "en-US", MarketName = "English", MarketNativeName = "English", ShortedCode = "en" }
        };

        public static IEnumerable<CategoryType> CategoryList = new List<CategoryType>
        {
            new CategoryType {Category = "All", SubCategory= "All" },
            new CategoryType { Category = "test1", SubCategory = "testSub2" },
            new CategoryType { Category = "Casino", SubCategory = "CasinoClubs" },
            new CategoryType { Category = "test2" }
        };

        public static IEnumerable<CurrencyType> CurrencyType = new List<CurrencyType>
        {
            new CurrencyType {Currency = "VND", CurrencyId=1 },
            new CurrencyType { CurrencyId = 2, Currency = "USD" },
            new CurrencyType { CurrencyId = 2, Currency = "Test" }
        };

        public static IEnumerable<Domain> DomainList = new List<Domain>
        {
            new Domain { DomainId = 1, DomainName = "abc.com", IsSsl = true },
            new Domain { DomainId = 2, DomainName = "1245.com", IsSsl = false }
        };

        public static IEnumerable<LandingPage> LandingPageList = new List<LandingPage>
        {
            new LandingPage { PageCode = "SignUp", PageName = "SingUp" },
            new LandingPage { PageCode = "Index", PageName = "Index" }
        };

        public static IEnumerable<BannerType> BannerTypeList = new List<BannerType>
        {
            new BannerType {Type = "Image", TypeId =1 },
            new BannerType {Type = "test", TypeId = 2 }
        };
        public static IEnumerable<SizeType> SizeTypes => new List<SizeType>
        {
            new SizeType { SizeId = 1, Size = "1080x1920" },
            new SizeType { SizeId = 2, Size = "640x256" },
        };

        public static UserSession Session = new UserSession
        {
            AffId = 1,
            AffType = "asian",
            GroupName = "admin",
            OnUserId = 1,
            PreferredLanguage = "vi-VN",
            UserName = "test"
        };

        public static IDictionary<string, string> LinkList = new Dictionary<string, string>
        {
            {"abc", "http:\\abc.com" },
            {"123", "http:\\123.com" }
        };

        public static string LogoImageBase64 = @"iVBORw0KGgoAAAANSUhEUgAAACoAAAAqCAYAAADFw8lbAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAACJNJREFUeNq0WGlsVNcVvm
                                                +dmefxLMyMt8ngFWO84A0MwQkB2hQESRsgVRWhNA0NCUrSqkVquilVm/KjVFFCpLSkUpWqVKhKSkKU0EWJIYGEYqgXXC
                                                +AsY3XweDxePb1LT13bCbDrG8GeqWjN8t9533vu+ec+91DoDRD0b7pQdJgep1QqVqRzEErFGG4EHwwSMu8RQL7BGyn752jvlSTyAxOvi/Oz61AfHhBLtA1z
                                                +4fbv3uviEkfxBgX1uy1ARkcGIC0wg3b16nSsxaRBCZXgwNHn/XIIkSgbIfursBurQ4YrmmtOx87SOPKklCNgZrpgmdfzjSLPK8vJCSvT75Gt
                                                oOkbJ8mb4Z3aNB0jS6DVSDeNaXbm6qPzx7K7YYKVEf+1sgFG62e7wD6P8wnhPGv+3YWbQlK0YxSLicJAlpLP4/m9tbrWJZq4plSopZeqGEZewy0poY8gUsAV
                                                FiUs1hkcThZwLYx3QnZj/OCBRA1sDlBJgqlc9pu4OsKDAED5SYRqpU7Fo5jL1htQ195vTWZpiGn3kcwG4AsAMplx5AauHyQSRk0jEkSUUTtoVRKVIC5dYg2UmYj4kCs
                                                Op0jL4KtlKON14Qag8OT3StMRkG5Sz9RY+vMouQrQI7BPZCAlBg8yG4PJNNAky6fSsNKtUUxdL6jC8mSXSW+fU8sPpXCIEv4hk9lG2mHqmvulaqUtQrWNoJ9dWUbu7
                                                rVtvls07vqiwfcRBsUzRGgU28fa3LFiixuP2xwRCPS6CQvmATYg5V6yFgdWMso88lBUKk34ZeHp4oqOCUkRjlGLpHn6/mUs2FGK3KtcSCnaWXMn1Hwt8UNUsoo
                                                VqEgyk9jPgC5slA0ByZThBiaTDUr+G4iiTJRPpFUZEj0MdwBcCMPowVXcJStbbZpClbWkb/0ljdA4y2xJS6RpahRiiSTGDvtRnblc9d3pocgOJV2kQmjU2anqD
                                                rV9dnFApJQiMUFkxQZ13xvzMEIdzFDrsBM9qW4LR9YxDFYZB4noqfd2hsWlejVvXG/66kaEGXz1GxHnq8/uq7ALoGAzXfkUCsYpSqqk5wKgYDCexdcLjLwZLvh
                                                SzTazHcM6VVTsYDpZqavffCs39RafXfI6AWzKgydrunV9VHY5MiiaSq9vDJV7o0AXfmjR5imDHozrESz97+6VpxLXl08/OtWQJV3LGtETr9NUik6HJxDCUuPg+
                                                FundZpHHFjYv75orb2qYurSQlKZ/IX4aYh/cgyTaDRNc8IguWI3F6BFG165DkmANaPUgK+Zz0qpY8vuNPNP5ucM8NAtDsRXbsjkKWmJNmZl2R5kpJgbGulRMiZ
                                                QxALkpAVoGoykZEWlYiankNIovKwEe5m6poCEf2LIZFpHmFlqxu8xM0ezdLH8CMToOVRgBodUm9faOhmEd+V998YLFo8yTlJCWRRfZZ5H1135cJp9FN6H/+K1L
                                                yzwWnz/+zKEgyi1Lt9HtIx/C9aoJc4VZqctlKbRjo5G2giMtTJpv1rebiKmlhVnND1OCXQg/sP25INq+e8oTeob2VRD6rPbH/Nxff9hljS19zoU57LV+lbMgB
                                                6DBe+p5ojHKcOqHMMNRMpYGLCOkFifGn8zYk5FXwHscYCnr/8wQxUR0vVG46nMUhnrfnAPS/GOjZKFAVp42fsa5UFz32zktsIJ03EdLxIzuhkuZn1hZKfl0Z6Y
                                                8/CKqn5xdyAfopBnoKLKI8CKUyL/bfPa1m9tiTLY1fAmXCmTz+kbdEVcyLzIQacsq7i57tOsCMY1Y8j5NW12H3xw6D6B9arlEM/eL+wks0SdxK4xL7O0Wr3x5zg
                                                oL6EKsUxDB3nBJf+kpVXex3u8Rk7BaMi6o6CJGQngizX6VslU/Q1sGfsGNrFhWWdP1JeqYFqEc/8nUGR9qN/HaLyqygiAHfmeFULt8Dle+9XUffQiS5Ga7GdCAmOk
                                                6HJ3umBkiGTcvsm+33SS8/UNKCl6vO2r9MKstbbDBJs8X9sx5XQ5FaUy3YC7u8ASs+eD7baKh/9xx/KYX0fisqnIHV094fNF9Iqktjxuh1KyO4XPWZZNCRT5
                                                yjAHQxxo20rnN8Ye7+Mr2pkBGUg+6ADy0dMDeVcJHjCzCKdpvpsJjQRUBngM3P7zgus9t2HM7YALvpNsqJfKc/XNk95YzEXYFaoXL4w9HtttWiizbDSjSKaK
                                                gpNz5YkaQd+eOEUyiz7WAHf+63f4OP30wKcmB81BT2uUwU6pUD9ovOy75Wy/oC/Hl9qT56StUq6aRtJNJoNBB5eVckb1Rc/x7YvJDqXI834Q3xigoJAirt+FBx
                                                3sDLl21T4zYkQr0nSaTnGEbOLWzbWj746WeRAg/205SdErr9JRtcHl0K4HNgEbEZ7vx3N4A1ZlX5RNHIX75yNZtbmMbVdfBiM/BxF7Dpju/2JnZU9zyFGX0BDm
                                                yXd1drb/167ow5z+P8Ge/0l0PkJO8AUhTKe/GHXZJrriLw/gczotPZQC7T93F7n27MAmtYnJ39Orv7tX8luE8qVfr73MrVTQqI5qOD84Gn3/QXbZ9GyoltSqddC
                                                vKFKRpSSLH1kQXSaLLRZfdVh7u6JckfKGCamwSCYSg5WhvscWbrK//Iqj/qOPZnfMN6nEd4VQC0QJBEA6VWpEymwEfvg66jECQFRy23YHXPhLt7BmWAxGJnM4Teyawb
                                                uUtg8cPwrvK7zeRCRAcQDNVMqti+pP2l3q4VfN/FCOPKHdtXgeJ28FeHM7XxjoE1AcgLGboy8gZss5jdX4JtjXT1faGrYpCvSSwzhkvcd55qihQL6w2rMDl5i12/rimJy26wAwD
                                                wrMz2UXYDAGON+T2w7YIn6JbCQml8vnPP7HWTOp02ye0hsL/jFQKAp7Lsc+U2ALBCCvFbBG8It9HxYQ33P4twOaTKy7tUu3fimmtbqoldS+WuAwA6c3ne/wQYAP4bCK37qqp9AAAAAElFTkSuQmCC";
    }
}