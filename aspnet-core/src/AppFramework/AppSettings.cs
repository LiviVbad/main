using Syncfusion.Licensing;

namespace AppFramework
{
    /// <summary>
    /// 应用程序配置文件
    /// </summary> 
    public class AppSettings
    {
        private bool isDarkTheme = false;
        private string themeName = "Material";

        static AppSettings()
        {
            Instance = new AppSettings();
        }

        public static AppSettings Instance { get; }

        public bool IsDarkTheme
        {
            get => isDarkTheme;
            set
            {
                isDarkTheme = value;
            }
        }

        public string ThemeName
        {
            get => themeName;
            set { themeName = value; }
        }

        public static void OnInitialized()
        {
            Syncfusion.SfSkinManager.SfSkinManager.ApplyStylesOnApplication = true;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjI5NjkxQDMyMzAyZTMxMmUzMG4yeGhZNm01STJSdnVKQVJiUHpzM3ZMUEc5K1hZTXd3TVFTbGZ1UERrQlU9");
        }
    }
}