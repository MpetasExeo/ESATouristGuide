using System;

using XFTemplateApp.Resources;

namespace XFTemplateApp.Helpers
{
    public class ApplicationExceptions
    {
        public static string ToString( Exception exception )
        {
            switch (exception)
            {
                case ServerException _:
                    return AppResources.DefaultErrorMessage;
                case NetworkException _:
                    return AppResources.NoInternetConnectionFound;
                case LocalizedException localizedException:
                    return localizedException.Message;
                case LocationException _:
                    return AppResources.DeviceLocationNA;
                case AggregateException aggregateException:
                    return ToString(aggregateException.InnerExceptions[0]);
                default:
                    return AppResources.DefaultErrorMessage;
            }
        }


        public class ServerException : Exception
        {
        }

        public class NetworkException : Exception
        {
        }

        public class LocationException : Exception
        {
        }

        public class LocalizedException : Exception
        {
            public LocalizedException( string message )
                : base(message)
            {
            }
        }
    }
}
