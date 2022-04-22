using System.Globalization;

namespace AppFramework.Common
{ 
    public interface ILocaleCulture
    { 
        CultureInfo GetCurrentCultureInfo();
         
        void SetLocale(CultureInfo ci);
    }
}