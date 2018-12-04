using System;
using System.Globalization;
using Xamarin.Forms;

namespace DatabaseDemo
{
    public class FormattedDateTimeConverter : IValueConverter
    {
        public static FormattedDateTimeConverter Instance = new FormattedDateTimeConverter();

        protected FormattedDateTimeConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fs = new FormattedString();
            fs.Spans.Add(new Span { Text = value.ToString(), FontAttributes = FontAttributes.Bold });
            return fs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
