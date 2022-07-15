using ESATouristGuide.Infrastructure;

using System;
using System.Globalization;

using Xamarin.Forms;

using static ESATouristGuide.Helpers.ApplicationExceptions;

namespace ESATouristGuide.Converters
{
    public class ExceptionToImageSourceConverter : IValueConverter
    {
        private static ImageResourceExtension imageResourceExtension;

        public object Convert( object value , Type targetType , object parameter , CultureInfo culture )
        {
            if (value == null)
            {
                return null;
            }

            if (imageResourceExtension == null)
            {
                imageResourceExtension = new ImageResourceExtension();
            }

            string imageName = value switch
            {
                ServerException _ => "Sample.Images.server.png",
                NetworkException _ => "Sample.Images.the_internet.png",
                _ => "Sample.Images.richmond.png",
            };
            imageResourceExtension.Source = imageName;
            return (ImageSource)imageResourceExtension.ProvideValue(null);

        }

        public object ConvertBack( object value , Type targetType , object parameter , CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
