using System;
using System.Diagnostics;
using System.Globalization;

using Xamarin.Forms;

namespace XFTemplateApp.Converters
{
    public class ScrollValueConverter : IValueConverter
    {

        public object Convert( object value , Type targetType , object parameter , CultureInfo culture )
        {
            NumberFormatInfo fmt = new NumberFormatInfo() { NegativeSign = "-" };

            double percentage = ( (double)value );

            Debug.WriteLine(percentage);

            string[] allParams = ( (string)parameter ).Split(( ';' ));
            double factor = Double.Parse(allParams[0] , fmt);
            double min = Double.Parse(allParams[1]);
            double max = Double.Parse(allParams[2]);
            bool reverse = bool.Parse(allParams[3]);
            double delayUntilPercentage = Double.Parse(allParams[4]);

            if (percentage == 0)
            {
                return min;
            }

            if (delayUntilPercentage > 0 && percentage < delayUntilPercentage)
            {
                return min;
            }

            percentage -= delayUntilPercentage;

            if (reverse)
            {
                percentage = 1 - ( percentage * factor );
                return ( percentage * max );
            }
            else
            {
                return ( percentage * max ) * factor;
            }
        }

        public object ConvertBack( object value , Type targetType , object parameter , CultureInfo culture )
        {
            //throw new NotImplementedException();
            return null;
        }
    }
}