using System;
using System.Windows.Data;
using System.Windows.Markup;
using AquaExplorer.Services;

namespace AquaExplorer.Converters
{
    class SizeAbbreviationConverter : MarkupExtension, IValueConverter
    {
        private readonly SizeAbbreviationService _service = new SizeAbbreviationService();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                long r = System.Convert.ToInt64(value);
                return _service.Abbreviate(r);
            }
            catch (Exception)
            {
                return value;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
