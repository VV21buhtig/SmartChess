using System;
using System.Globalization;
using System.Windows.Data;

namespace SmartChess.Resources.Converters
{
    public class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime endTime)
            {
                var startTime = DateTime.Now.AddHours(-1);
                var duration = endTime - startTime;
                return $"{duration.Minutes} мин {duration.Seconds} сек";
            }
            return "Не завершена";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}