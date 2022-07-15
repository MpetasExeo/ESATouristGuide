using System;
using System.Globalization;

using Xamarin.Forms;

using XFTemplateApp.Helpers;

namespace XFTemplateApp.Converters
{
    public class ExceptionToErrorMessageConverter : IValueConverter
    {
        public object Convert( object value , Type targetType , object parameter , CultureInfo culture )
        {
            Exception exception = value as Exception;

            if (value == null)
            {
                return null;
            }

            return ApplicationExceptions.ToString(exception);
        }

        public object ConvertBack( object value , Type targetType , object parameter , CultureInfo culture )
        {
            // One-Way converter only
            throw new NotImplementedException();
        }
    }
}
