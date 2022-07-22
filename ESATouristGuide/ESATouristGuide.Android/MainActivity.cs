using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

using FFImageLoading.Forms.Platform;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps.Android;

[assembly: Dependency(typeof(ESATouristGuide.Droid.Models.Enviroment))]
namespace ESATouristGuide.Droid
{
    [Activity(Label = "ESATouristGuide" , Icon = "@mipmap/icon" , Theme = "@style/MainTheme" , MainLauncher = false , ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);


            Xamarin.Essentials.Platform.Init(this , savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this , savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this , savedInstanceState);
            Android.Glide.Forms.Init(this , debug: true);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            CachedImageRenderer.InitImageViewHandler();


            PlatformConfig platformConfig = new PlatformConfig
            {
                BitmapDescriptorFactory = new CachingNativeBitmapDescriptorFactory()
            };

            Xamarin.FormsGoogleMaps.Init(this , savedInstanceState , platformConfig); // initialize for Xamarin.Forms.GoogleMaps

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode , string[] permissions , [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode , permissions , grantResults);

            base.OnRequestPermissionsResult(requestCode , permissions , grantResults);
        }
    }
}