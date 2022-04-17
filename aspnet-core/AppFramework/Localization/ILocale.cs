using System.Globalization;

namespace AppFramework.Localization
{ 
    public interface ILocale
    { 
        CultureInfo GetCurrentCultureInfo();
         
        void SetLocale(CultureInfo ci);
    }
}