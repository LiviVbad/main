using System;

namespace AppFramework
{
    /// <summary>
    /// 应用程序中使用的一些常量
    /// </summary>
    public class AppConsts
    {
        /// <summary>
        /// 默认页面大小
        /// </summary>
        public const int DefaultPageSize = 10;

        /// <summary>
        /// 默认分页组件显示的按钮数量
        /// </summary>
        public const int NumericButtonCount = 10;

        /// <summary>
        /// 最大的页面大小
        /// </summary>
        public const int MaxPageSize = 1000;

        /// <summary>
        /// SimpleStringCipher 解密/加密操作的默认密码短语
        /// </summary>
        public const string DefaultPassPhrase = "ca62ad7126ec4688a178b4063cdfd0cc";

        public const int ResizedMaxProfilePictureBytesUserFriendlyValue = 1024;

        public const int MaxProfilePictureBytesUserFriendlyValue = 5;

        public const string TokenValidityKey = "token_validity_key";
        public const string RefreshTokenValidityKey = "refresh_token_validity_key";
        public const string SecurityStampKey = "AspNet.Identity.SecurityStamp";

        public const string TokenType = "token_type";

        public static string UserIdentifier = "user_identifier";

        public const string ThemeDefault = "default";
        public const string Theme2 = "theme2";
        public const string Theme3 = "theme3";
        public const string Theme4 = "theme4";
        public const string Theme5 = "theme5";
        public const string Theme6 = "theme6";
        public const string Theme7 = "theme7";
        public const string Theme8 = "theme8";
        public const string Theme9 = "theme9";
        public const string Theme10 = "theme10";
        public const string Theme11 = "theme11";
        public const string Theme12 = "theme12";
        public const string Theme13 = "theme13";

        public static TimeSpan AccessTokenExpiration = TimeSpan.FromDays(1);
        public static TimeSpan RefreshTokenExpiration = TimeSpan.FromDays(365);

        public const string DateTimeOffsetFormat = "yyyy-MM-ddTHH:mm:sszzz";
    }
}