using Android.App;
using Android.Content;
using Android.OS;

using AndroidX.AppCompat.App;

namespace ESATouristGuide.Droid
{
    [Activity(Label = "SplashActivity" , Icon = "@mipmap/ic_launcher" ,
        Theme = "@style/SplashTheme" ,
        MainLauncher = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate( Bundle savedInstanceState )
        {
            base.OnCreate(savedInstanceState);
            Intent intent = new Intent(this , typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.AddFlags(ActivityFlags.SingleTop);
            StartActivity(intent);
            Finish();
        }



    }
}