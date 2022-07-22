
using ESATouristGuide.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ESATouristGuide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailsPage : ContentPage
    {
        public ItemDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = this.BindingContext as ItemDetailsViewModel;
            vm.Load();
        }
    }
}