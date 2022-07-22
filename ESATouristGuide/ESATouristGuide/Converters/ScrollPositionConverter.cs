
using System;
using System.Globalization;

using Xamarin.Forms;

namespace ESATouristGuide.Converters
{
    public class ScrollPositionConverter : IValueConverter
    {

        public object Convert(object value , Type targetType , object parameter , CultureInfo culture)
        {
            NumberFormatInfo fmt = new NumberFormatInfo() { NegativeSign = "-" };

            var position = (double)value;

            var allParams = ((string)parameter).Split(';');
            var factor = double.Parse(allParams[0] , fmt);
            var min = double.Parse(allParams[1]);
            var max = double.Parse(allParams[2]);
            var reverse = bool.Parse(allParams[3]);
            var delayUntilPosition = double.Parse(allParams[4]);

            if (position == 0)
            {
                return min;
            }

            position *= factor;

            if (delayUntilPosition > 0 && position < delayUntilPosition)
            {
                return min;
            }

            return Math.Max(0 , min - position);

            //position = position - delayUntilPosition;

            //if(reverse)
            //{
            //    position = 1 - (position * factor);
            //    return (position * max);
            //}
            //else
            //{
            //    return Math.Min(position * factor, max);
            //}
        }

        public object ConvertBack(object value , Type targetType , object parameter , CultureInfo culture)
        {
            //throw new NotImplementedException();
            return null;
        }
    }
}