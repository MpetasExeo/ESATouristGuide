﻿
using System;
using System.Globalization;

using Xamarin.Forms;

namespace ESATouristGuide.Converters
{
    public class ScrollPositionConverter : IValueConverter
    {

        public object Convert( object value , Type targetType , object parameter , CultureInfo culture )
        {
            NumberFormatInfo fmt = new NumberFormatInfo() { NegativeSign = "-" };

            double position = (double)value;

            string[] allParams = ( (string)parameter ).Split(';');
            double factor = double.Parse(allParams[0] , fmt);
            double min = double.Parse(allParams[1]);
            double max = double.Parse(allParams[2]);
            bool reverse = bool.Parse(allParams[3]);
            double delayUntilPosition = double.Parse(allParams[4]);

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

        public object ConvertBack( object value , Type targetType , object parameter , CultureInfo culture )
        {
            //throw new NotImplementedException();
            return null;
        }
    }
}