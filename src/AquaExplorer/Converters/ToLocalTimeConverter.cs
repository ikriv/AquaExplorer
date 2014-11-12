using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace AquaExplorer.Converters
{
    class ToLocalTimeConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is DateTime)) return value;
            var date = (DateTime) value;
            return date.ToLocalTime();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is DateTime)) return value;
            var date = (DateTime) value;
            return date.ToUniversalTime();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
