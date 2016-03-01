using System;
using System.Globalization;
using System.Windows.Data;

namespace Cortex.Converter
{
    public class SingleLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string) value;
            if (str != null)
            {
                var pos = str.IndexOf('\n');
                return (pos > 0) ? str.Substring(0, pos) : str;
            }
            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
