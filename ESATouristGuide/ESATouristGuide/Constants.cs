using System;
using System.IO;

namespace ESATouristGuide
{
    public class Constants
    {
        public static readonly string GoogleApiKey = "AIzaSyAbe9MnGMU3M9DP3BHmSYqkKpW8qRSNePU";

        public static readonly string APIConnectionBaseAddress= "https://esa.exeo.site/";


        // {AssemblyName}.Json.{FileName}.json
        public static readonly string LightThemeMapPath = "ESATouristGuide.Json.travelstyle.json";
        public static readonly string DarkThemeMapPath = "ESATouristGuide.Json.darkstyle.json";

        public static readonly string GoogleMapsBaseAddress = "https://maps.googleapis.com";
        public static readonly string WeatherApiBaseAddress = "https://api.openweathermap.org";

        public const string DB_Name = "esapoisdb3.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath , DB_Name);
            }
        }

    }
}
