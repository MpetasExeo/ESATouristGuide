using Xamarin.Forms;

using XFTemplateApp.Views;

namespace XFTemplateApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(GoogleMapsPage) , typeof(GoogleMapsPage));
        }

    }
}
