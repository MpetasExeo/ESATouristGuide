
using System;
using System.Diagnostics;
using System.Globalization;

using Xamarin.Forms;

namespace ESATouristGuide.Converters
{
    public class ScrollValueConverter : IValueConverter
    {

        public object Convert(object value , Type targetType , object parameter , CultureInfo culture)
        {
            NumberFormatInfo fmt = new NumberFormatInfo() { NegativeSign = "-" };

            var percentage = (double)value;

            Debug.WriteLine(percentage);

            var allParams = ((string)parameter).Split(';');
            var factor = double.Parse(allParams[0] , fmt);
            var min = double.Parse(allParams[1]);
            var max = double.Parse(allParams[2]);
            var reverse = bool.Parse(allParams[3]);
            var delayUntilPercentage = double.Parse(allParams[4]);

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
                percentage = 1 - (percentage * factor);
                return percentage * max;
            }
            else
            {
                return percentage * max * factor;
            }
        }

        public object ConvertBack(object value , Type targetType , object parameter , CultureInfo culture)
        {
            //throw new NotImplementedException();
            return null;
        }
    }
}