using Abp.Notifications;
using AppFramework.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AppFramework.Converters
{
    public class NotificationMessage
    {
        public string SourceName { get; set; }

        public string Name { get; set; }
    }

    public class NotificationToMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                StringBuilder sb = new StringBuilder();

                var utf = value as UserNotification;
                if (utf != null)
                {
                    foreach (var item in utf.Notification.Data.Properties)
                    {
                        if (item.Key.Equals("Message"))
                        {
                            var val = JsonConvert.DeserializeObject<NotificationMessage>(item.Value.ToString());
                            if (val != null)
                            {
                                sb.Append(Local.Localize(val.Name));
                            }
                        }
                    }
                }
                return sb.ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
