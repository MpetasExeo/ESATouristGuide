using Android.OS;

using AndroidX.Core.View;

using System;
using System.Threading.Tasks;

using Xamarin.Essentials;

using XFTemplateApp.Interfaces;

namespace XFTemplateApp.Droid.Models
{
    public class Enviroment : IEnvironment
    {
        public async void SetStatusBarColor( System.Drawing.Color color , bool darkStatusBarTint )
        {
            try
            {
                if (Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Lollipop)
                {
                    return;
                }

                Android.App.Activity activity = Platform.CurrentActivity;
                Android.Views.Window window = activity.Window;

                //this may not be necessary(but may be fore older than M)
                window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
                window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);


                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
                {
                    await Task.Delay(50).ConfigureAwait(true);
                    WindowCompat.GetInsetsController(window , window.DecorView).AppearanceLightStatusBars = darkStatusBarTint;
                }

                window.SetStatusBarColor(color.ToPlatformColor());
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}